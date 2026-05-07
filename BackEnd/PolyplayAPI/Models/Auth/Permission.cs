using System.ComponentModel.DataAnnotations;

namespace PolyplayAPI.Models.Auth
{
    public class Permission
    {
        [Key]
        public long Id { get; set; }
        public string Feature { get; set; } = null!;
        public string Action { get; set; } = null!;
    }
}
