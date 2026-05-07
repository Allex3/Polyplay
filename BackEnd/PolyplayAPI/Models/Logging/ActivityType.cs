using System.ComponentModel.DataAnnotations;

namespace PolyplayAPI.Models.Logging
{
    public class ActivityType
    {
        [Key] public long Id { get; set; }
        public string Info { get; set; } = null!;

        
    }
}
