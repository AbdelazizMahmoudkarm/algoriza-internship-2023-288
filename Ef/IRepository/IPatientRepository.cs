﻿using algoriza_internship_288.Domain.Models;
using Domain.DtoClasses.Patient;
using Repository.Repository;

namespace Repository.IRepository
{
    public interface IPatientRepository : IBaseRepository<ApplicationUser>
    {
        public Task<bool> AddAsync(AddPatientDto patientModel);
    }
}
