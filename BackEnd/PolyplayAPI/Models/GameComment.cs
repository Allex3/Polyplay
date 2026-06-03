using PolyplayAPI.Models.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolyplayAPI.Models
{
    public class GameComment
    {
        [Key] public long Id { get; set; }
        [Required]
        public long GameId { get; set; } // foreign key

        public string? UserId { get; set; }

        public string UserName { get; set; } = null!;
        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string? Body { get; set; }

        public Game? Game { get; set; }
        public User? User { get; set; }

    }
}
