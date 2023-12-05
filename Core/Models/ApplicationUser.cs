using algoriza_internship_288.Core.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace algoriza_internship_288.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public Gender Gender { get; set; }
        public string? Image { get; set; }
        public DateTime DateOfBirth { get; set; }
        public virtual Doctor Doctor { get; set; }
        public DateTime DateOfAdd { get; set; }
        public virtual List<Booking> Bookings { get; set; }
  
    }

}
