using Microsoft.AspNetCore.Identity;

namespace PolyplayAPI.Models.Auth
{
    public class User : IdentityUser
    {
        public bool WantsToReceiveGameMails { get; set; }

        public ICollection<Game>? Games { get; set; }

        public ICollection<GameComment> GameComments { get; set; }
    }
}

