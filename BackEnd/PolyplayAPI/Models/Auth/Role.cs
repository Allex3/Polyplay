using System.ComponentModel.DataAnnotations;

namespace PolyplayAPI.Models.Auth
{
    public class Role
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
