using PolyplayAPI.Models.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolyplayAPI.Models.Logging
{
    public class MaliciousActivity
    {
        [Key]
        public long Id { get; set; }
        public string UserName { get; set; }
        [ForeignKey(nameof(ActivityType))]
        public long ActivityTypeId { get; set; }

        public ActivityType ActivityType { get; set; } = null!;
        public User User { get; set; } = null!;
        public string? IpAddress { get; set; }
        public string? Info { get; set; }
    }
}
