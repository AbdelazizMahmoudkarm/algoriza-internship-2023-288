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
        //public int GetByName(string name)
        //{
        //    int? specializeId = _context.Specializations
        //        .FirstOrDefault(x => x.Name.Equals(name))?.Id;
        //    if (specializeId.HasValue && specializeId.Value > 0)
        //        return specializeId.Value;
        //    else
        //        return 0;
        //}
        //public Specialization AddByName(string name)
        //{
        //    return new Specialization
        //    {
        //        Name = name,
        //    };
        //}
        public string GetSpecializeNameByDoctorId(int doctorId)
        {
            return _context.Doctors
                            .Where(x => x.Id.Equals(doctorId))
                            .Select(x =>_arabic? x.Specialization.ArName:x.Specialization.Name )
                            .FirstOrDefault();
        }
    }
}
