using System.ComponentModel.DataAnnotations;
using algoriza_internship_288.Domain.Models.Enums;

namespace algoriza_internship_288.Domain.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        [Required]
        public int DoctorId { get; set; }
        [Required]
        public virtual Days ExistingDay { get; set; }
        public virtual List<Time> Times { get; set; }

        //public int Price { get; set; }
        // public int ExistingDayId { get { return (int)ExistingDay; } set { ExistingDay = (Days)value; } }
        // public virtual Doctor Doctor { get; set; }
    }
}
