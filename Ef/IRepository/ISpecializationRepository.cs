using algoriza_internship_288.Core.Models;
using Repository.Repository;

namespace Repository.IRepository
{
    public interface ISpecializationRepository
    {
        public int GetByName(string name);
        public Specialization AddByName(string name);
        public string GetSpecializeNameByDoctorId(int doctorId);
    }
}
