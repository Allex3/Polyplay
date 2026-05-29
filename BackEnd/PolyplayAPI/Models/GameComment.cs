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

        [ForeignKey(nameof(User))]
        [Required] public string UserId { get; set; } = null!; // foreign key
        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Body { get; set; }

        //TODO PLS MAKE DTO OF USER
        //public Game Game { get; set; } = null!; // references, so it discovers the relationship by convention
        public User User { get; set; } = null!;

    }
}
