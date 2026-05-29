using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public AuthenticationController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, PolyplayDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
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

            return Created(nameof(Register), new { Message = $"User {register.UserName} created" });
        }

        private async Task<AuthenticationResultViewModel> GenerateJwtToken(User user)
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

            // same as in program.cs
            var authSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWT:Secret") ?? "uhhhwelpsecretkeynotwork?"));

            var token = new JwtSecurityToken(issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(6.7), // 6.7 minutes expiry time
                claims: authClaims, // the client will send this to use for authorization
                // the actual secret and the algorithm to secure the token
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            // serializes the jwtSecuriryToken into a compact format, but it's the same, i think
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);


            // the refresh token creation
            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsRevoked = false,
                UserId = user.Id,
                DateAdded = DateTime.UtcNow,
                DateExpire = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid().ToString() // the actual token value
            };
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            // the response VM that we send to the browser
            var response = new AuthenticationResultViewModel()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = token.ValidTo // comes from "Expires" in JwtSecurityToken
            };
            return response;
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Login([FromBody] LoginViewModel login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);

            // user exists and password is ok
            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
            {
                var tokenValue = await GenerateJwtToken(user);

                return Ok(tokenValue); // is this stored automatically? hope so
            }

            return Unauthorized(new {User = "Invalid username or password"});
        }
    }
}
