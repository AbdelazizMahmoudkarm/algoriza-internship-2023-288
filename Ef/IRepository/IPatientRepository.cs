using Domain.DtoClasses.Patient;
using Repository.Repository;

namespace Repository.IRepository
{
    public interface IPatientRepository : IBaseRepository
    {
        public Task<bool> AddAsync(AddPatientDto patientModel);
    }
}
