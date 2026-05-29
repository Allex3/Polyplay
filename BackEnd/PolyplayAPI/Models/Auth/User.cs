using Microsoft.AspNetCore.Identity;

namespace PolyplayAPI.Models.Auth
{
    public class User : IdentityUser
    {
        public bool WantsToReceiveGameMails { get; set; }
    }
}

