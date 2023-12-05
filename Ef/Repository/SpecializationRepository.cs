using algoriza_internship_288.Core.Models;
using algoriza_internship_288.Ef.DAL;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;

namespace Repository.Repository
{
    public class SpecializationRepository : ISpecializationRepository
    {
        private readonly AppDbContext _context;
        public SpecializationRepository(AppDbContext context)
        {
            _context = context;
        }
        public int GetByName(string name)
        {
            int? specializeId = _context.Specializations
                .FirstOrDefault(x => x.Name.Equals(name))?.Id;
            if (specializeId.HasValue && specializeId.Value > 0)
                return specializeId.Value;
            else
                return 0;
        }
        public Specialization AddByName(string name)
        {
            return new Specialization
            {
                Name = name,
            };
        }
        public string GetSpecializeNameByDoctorId(int doctorId)
        {
            return _context.Doctors
                            .Where(x => x.Id.Equals(doctorId))
                            .Select(x => x.Specialization.Name)
                            .FirstOrDefault();
        }
    }
}
