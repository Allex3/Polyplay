using PolyplayAPI.Models;
using PolyplayAPI.Models.Auth;
using System.ComponentModel.DataAnnotations;

namespace PolyplayAPI.ViewModels.Games
{
    public class GameDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public DateTime PostedDate { get; set; }
        public string? Description { get; set; }
        public string MainTag { get; set; }
        public string ThumbnailPath { get; set; }
        public double Rating { get; set; }
        public string? Developer { get; set; }

        public bool IsPublished { get; set; } = false;

    }
}
