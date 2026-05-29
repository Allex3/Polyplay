using PolyplayAPI.Models.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PolyplayAPI.Models.Logging
{
    public class UserActivity
    {
        [Key]
        public long Id { get; set; }
        //TODO USEER FOREIGN KEY LATER
        public string UserName { get; set; } = null!;
        [ForeignKey(nameof(ActivityType))]
        public long ActivityTypeId { get; set; }

        public ActivityType ActivityType { get; set; } = null!;

        public DateTime? ActivityTimestamp { get; set; }
        public string? IpAddress { get; set; }
        public string? AdditionalInfo { get; set; }
    }
}
