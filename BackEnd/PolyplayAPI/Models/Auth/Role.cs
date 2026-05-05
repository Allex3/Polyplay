using System.ComponentModel.DataAnnotations;

namespace PolyplayAPI.Models.Auth
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public List<User> Users { get; } = [];
        public List<Permission> Permissions { get; } = [];


    }
}
