using System.ComponentModel.DataAnnotations;

namespace PolyplayAPI.Models
{
    public class Game
    {
        public long Id { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 1)]
        public string? Name { get; set; }
        public DateTime PostedDate { get; set; }
        [Required]
        [StringLength(1000, MinimumLength = 2)]
        public string? Description { get; set; }
        public string MainTag { get; set; }
        public string ThumbnailPath { get; set; }
        public double Rating { get; set; }
        [Required]
        public string? Developer { get; set; }

        public bool IsPublished { get; set; } = false;
        public List<GameComment> GameComments { get; } = new List<GameComment>();
        public virtual List<User> UsersWhoFavorited { get; set; } = []; // many to many basic relationship from User to Games

    }
}
