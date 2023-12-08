using System.ComponentModel.DataAnnotations;

namespace algoriza_internship_288.Domain.Models
{
    public class Specialization
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ArName { get; set; }
        public virtual List<Doctor> Doctors { get; set; }
    }
}
