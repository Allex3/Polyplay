using PolyplayAPI.Models;
using PolyplayAPI.Models.Auth;
using System.ComponentModel.DataAnnotations;

namespace PolyplayAPI.ViewModels.Games
{
    public class GameCommentDto
    { 
        public long Id { get; set; }
        public long GameId { get; set; } // foreign key

        public string UserName { get; set; } = null!;
        public string? Body { get; set; }
    }
}
