using System.ComponentModel.DataAnnotations;

namespace PolyplayAPI.Models
{
    public class GameComment
    {
        [Key] public long Id { get; set; }
        [Required]
        public long GameId { get; set; } // foreign key
        [Required]
        public long UserId { get; set; } // foreign key
        [Required]
        [StringLength(1000,MinimumLength = 1)]
        public string Body { get; set; }

        public Game Game { get; set; } = null!; // references, so it discovers the relationship by convention
        public User User { get; set; } = null!;

    }
}
