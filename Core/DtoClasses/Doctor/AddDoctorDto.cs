using algoriza_internship_288.Core.Models.Enums;
using Domain.DtoClasses;

namespace Domain.DtoClasses.Doctor
{
    public class AddDoctorDto : BaseDto
    {
        public string SpecializeName { get; set; }

        //public virtual int GenderId { get { return (int)Gender; } set { Gender = (Gender)value; } }



        //public UserType UserType { get; set; }

        //public virtual int UserTypeId { get { return (int)UserType; } set { UserType = (UserType)value; } }


        // public virtual Specialization Specialization { get; set; }
    }
}
