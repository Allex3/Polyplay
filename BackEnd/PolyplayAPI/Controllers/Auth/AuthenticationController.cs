using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PolyplayAPI.Filters;
using PolyplayAPI.Models;
using PolyplayAPI.Models.Auth;
using PolyplayAPI.ViewModels.Auth;

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
                SecurityStamp = Guid.NewGuid().ToString()
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
    }
}
