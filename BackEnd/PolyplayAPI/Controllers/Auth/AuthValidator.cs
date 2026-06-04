using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PolyplayAPI.Controllers.Auth
{
    public static class AuthValidator
    {
        public static string Authenticate(string token)
        {
            var key = new SymmetricSecurityKey("THEREISNoWayAnyoneWILLGUESSMYSECRETKEYLMAO"u8.ToArray());
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            List<Exception> validationFailures = null;
            SecurityToken validatedToken;
            var validator = new JwtSecurityTokenHandler();

            // These need to match the values used to generate the token
            TokenValidationParameters validationParameters = new TokenValidationParameters();
            validationParameters.ValidIssuer = "https://localhost:7114";
            validationParameters.ValidAudience = "User";
            validationParameters.IssuerSigningKey = key;
            validationParameters.ValidateIssuerSigningKey = true;
            validationParameters.ValidateAudience = true;

            if (validator.CanReadToken(token))
            {
                ClaimsPrincipal principal;
                try
                {
                    // This line throws if invalid
                    principal = validator.ValidateToken(token, validationParameters, out validatedToken);

                    // If we got here then the token is valid
                    if (principal.HasClaim(c => c.Type == ClaimTypes.Role))
                    {
                        foreach (var claim in principal.Claims.Where(c => c.Type == ClaimTypes.Role))
                        {
                            if (claim.Value == "Admin")
                                return "Admin";
                        }
                    }

                    return "User";
                }
                catch (Exception e)
                {
                    // _logger.LogError(null, e);
                }
            }

            return string.Empty;
        }
    }
}
