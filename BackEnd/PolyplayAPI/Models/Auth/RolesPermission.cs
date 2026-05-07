using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolyplayAPI.Models.Auth
{
    [PrimaryKey(nameof(RoleId), nameof(PermissionId))]
    public class RolesPermission
    {
        [ForeignKey(nameof(Role))]
        public long RoleId { get; set; }
        [ForeignKey(nameof(Permission))]
        public long PermissionId { get; set; }

        public Role Role { get; set; } = null!;

        public Permission Permission { get; set; } = null!;
    }
}
