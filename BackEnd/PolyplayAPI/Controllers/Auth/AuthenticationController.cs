using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using PolyplayAPI.Filters;
using PolyplayAPI.Models;
using PolyplayAPI.Models.Auth;
using PolyplayAPI.ViewModels.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PolyplayAPI.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PolyplayDbContext _context;
        private readonly IConfiguration _configuration;

        // refresh tokens:
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthenticationController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, PolyplayDbContext context, IConfiguration configuration, TokenValidationParameters tokenValidationParameters)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
            _tokenValidationParameters = tokenValidationParameters;
        }

        // user register account
        [HttpPost("register")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Register([FromBody] UserRegisterViewModel register)
        {
            var userExists = await _userManager.FindByNameAsync(register.UserName);

            if (userExists != null)
            {
                return BadRequest(new {User = $"User {register.UserName} already exists"});
            }

            // user does not exist
            User newUser = new User()
            {
                Email = register.Email,
                UserName = register.UserName,
                SecurityStamp = Guid.NewGuid().ToString(),
                WantsToReceiveGameMails = register.WantsToReceiveGameMails
            };

            // password is hashed automatically in the user manager
            var result = await _userManager.CreateAsync(newUser, register.Password);

            if (!result.Succeeded)
            {
                var errorJson = new Dictionary<string, string>();
                foreach (var error in result.Errors)
                {
                    errorJson.Add(error.Code, error.Description);
                }
                return BadRequest(errorJson);
            }

            await _userManager.AddToRoleAsync(newUser, UserRoles.User);

            return Created(nameof(Register), new { Message = $"User {register.UserName} created" });
        }

        private async Task<AuthenticationResultViewModel> GenerateJwtTokenAsync(User user, string existingRefreshToken = "")
        {

            //  so a claim is who authorizes the user? Like what claims the user has to data?
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add User Roles
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            //TODO add permission based on what games the user has, idk how tho

            // same as in program.cs
            var authSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWT:Secret") ?? "uhhhwelpsecretkeynotwork?"));

            var token = new JwtSecurityToken(issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(5), // 6.7 minutes expiry time
                claims: authClaims, // the client will send this to use for authorization
                // the actual secret and the algorithm to secure the token
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            // serializes the jwtSecuriryToken into a compact format, but it's the same, i think
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            var newRefreshToken = new RefreshToken();

            if (string.IsNullOrEmpty(
                    existingRefreshToken)) // we don't have, first user login in 6 months, only then add to db
            {
                // the refresh token creation
                newRefreshToken = new RefreshToken()
                {
                    JwtId = token.Id,
                    IsRevoked = false,
                    UserId = user.Id,
                    DateAdded = DateTime.UtcNow,
                    DateExpire = DateTime.UtcNow.AddMonths(6),
                    Token = Guid.NewGuid().ToString() // the actual token value
                };

                await _context.RefreshTokens.AddAsync(newRefreshToken);
                await _context.SaveChangesAsync();
            }


            // the response VM that we send to the browser
            var response = new AuthenticationResultViewModel()
            {
                Token = jwtToken,
                RefreshToken = (string.IsNullOrEmpty(
                existingRefreshToken)) ? newRefreshToken.Token : existingRefreshToken,
                ExpiresAt = token.ValidTo // comes from "Expires" in JwtSecurityToken
            };
            return response;
        }

        private async Task<AuthenticationResultViewModel> VerifyAndGenerateTokenAsync(
            TokenRequestViewModel tokenRequest)
        {
            // handle the jwt
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // check JWT token format: reads and validates the token in compact form
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters,
                    out var validatedToken);

                // check encryption algorithm
                if (validatedToken is JwtSecurityToken jwtSecurityToken) // check while casting it
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase);

                    if (result == false) return null; // bad algorithm
                }

                // check expiration date
                var utcExpirationDate = long.Parse(tokenInVerification.Claims
                    .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expirationDate = UnixTimeStampToDateTimeInUtc(utcExpirationDate);

                if (expirationDate > DateTime.UtcNow)
                    throw new Exception("Token has not expired yet, don't refresh it!");

                // check that Refresh Token exists in the DB
                var dbRefreshToken =
                    await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == tokenRequest.RefreshToken);

                if (dbRefreshToken == null)
                    throw new Exception("Refresh Token Does not exist in the Database");

                // check 5: validate Id
                var jti = tokenInVerification.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

                if (dbRefreshToken.JwtId != jti)
                    throw new Exception("Refresh token does not match with jti");

                // check 6
                if (dbRefreshToken.DateExpire <= DateTime.UtcNow)
                    throw new Exception("Your refresh token has expired, please login again");

                // check 7 
                if (dbRefreshToken.IsRevoked)
                    throw new Exception("Your refresh token has been revoked");

                // ALL GOOD - generate new token using existing refresh token
                var correspondingUser = await _userManager.FindByIdAsync(dbRefreshToken.UserId);

                var newTokenResponse = await GenerateJwtTokenAsync(correspondingUser, tokenRequest.RefreshToken);

                return newTokenResponse;
            }
            catch (SecurityTokenExpiredException ex)
            {
                var dbRefreshToken =
                    await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == tokenRequest.RefreshToken);

                // generate new token using existing refresh token
                var correspondingUser = await _userManager.FindByIdAsync(dbRefreshToken.UserId);

                var newTokenResponse = await GenerateJwtTokenAsync(correspondingUser, tokenRequest.RefreshToken);

                return newTokenResponse;
            }

           
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestViewModel tokenRequest)
        {
            try
            {
                var result = await VerifyAndGenerateTokenAsync(tokenRequest);

                if (result == null)
                    return BadRequest(new { Error = "Invalid Tokens" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Login([FromBody] LoginViewModel login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);

            // user exists and password is ok
            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
            {
                var tokenValue = await GenerateJwtTokenAsync(user, "");

                return Ok(tokenValue); // will be stored in client.. soemhow
            }

            return Unauthorized(new {User = "Invalid username or password"});
        }

        private DateTime UnixTimeStampToDateTimeInUtc(long unixTimeStamp)
        {
            var dateTimeValue = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeValue = dateTimeValue.AddSeconds(unixTimeStamp);
            return dateTimeValue;
        }
    }

}
