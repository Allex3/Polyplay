using System.ComponentModel.DataAnnotations;

namespace PolyplayAPI.ViewModels.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
