using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolyplayAPI.Models.Logging
{
    public class MaliciousActivity
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey(nameof(User))]
        public long UserId { get; set; }
        [ForeignKey(nameof(ActivityType))]
        public long ActivityTypeId { get; set; }

        public ActivityType ActivityType { get; set; } = null!;
        public User User { get; set; } = null!;
        public string? IpAddress { get; set; }
        public string? Info { get; set; }
    }
}
