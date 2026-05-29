namespace PolyplayAPI.ViewModels.Auth
{
    public class AuthenticationResultViewModel
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}
