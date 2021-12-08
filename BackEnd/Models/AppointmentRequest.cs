using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models
{
    public class AppointmentRequest
    {
        [Required]
        public DateTime Start { set; get; }
        [Required]
        public DateTime End { set; get; }
        [Required]
        public string PatintName { get; set; }
        
    }
}
