using algoriza_internship_288.Domain.Models;
using Repository.Repository;

namespace Repository.IRepository
{
    public interface ISpecializationRepository
    {
        //public int GetByName(string name);
        //public Specialization AddByName(string name);
        public string GetSpecializeNameByDoctorId(int doctorId);
    }
}
