using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PolyplayAPI.Models
{
    [Index(nameof(Username), IsUnique=true)] // unique property on username
    public class User
    {
        [Key] public long Id { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1)]
        
        public required string Username { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must only contain legal characters: a-z, A-Z, 0-9, @.,!, it should have at least one digit, no space, and at least one symbol [@$!%*?&]")]
        public required string Password { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(250)]
        public required string Email { get; set; }
        public ICollection<GameComment> GameComments { get; } = new List<GameComment>();

        public virtual ICollection<Game> FavoriteGames { get; set; } = []; // many to many basic relationship from User to Games

        // symmetrical self-referencing many to many relationship require 1 list, not 2
        // limitations: the saem navigation (Friends) cannot be use for both ends
        // so we need, when adding a friend, to add it to BOTH of the users' lists
        public virtual ICollection<User> Friends { get; } = []; 
        //TODO PLEASE MAKE DTO OF USER


    }
}
