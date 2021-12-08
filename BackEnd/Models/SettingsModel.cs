using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models
{
    public class SettingsModel
    {
        [Required]
        public string SessionPeriod { get; set; }
        [Required]
        public string WaitPeriod { get; set; }
        [Required]
        public string RestPeriod { get; set; }
    }
}
