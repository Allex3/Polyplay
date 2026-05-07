using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolyplayAPI.Models.Auth
{
    [PrimaryKey(nameof(UserId),nameof(RoleId))]
    public class UsersRole
    {
        [ForeignKey(nameof(User))]
        public long UserId { get; set; }
        [ForeignKey(nameof(Role))]
        public long RoleId { get; set; }

        public User User { get;set; }

        public Role Role { get; set; }
    }
}
