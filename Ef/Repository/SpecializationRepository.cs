using algoriza_internship_288.Repository.DAL;
using Repository.IRepository;

namespace Repository.Repository
{
    public class SpecializationRepository : ISpecializationRepository
    {
        private readonly AppDbContext _context;
        private readonly bool _arabic;
        public SpecializationRepository(AppDbContext context,bool arabic)
        {
            _context = context;
            _arabic = arabic;
        }
        public string GetSpecializeNameByDoctorId(int doctorId)
        {
            return _context.Doctors
                            .Where(x => x.Id.Equals(doctorId))
                            .Select(x =>_arabic? x.Specialization.ArName:x.Specialization.Name )
                            .FirstOrDefault();
        }
    }
}
