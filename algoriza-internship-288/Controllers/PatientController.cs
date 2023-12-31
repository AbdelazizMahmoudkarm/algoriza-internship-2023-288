﻿using algoriza_internship_288.Domain.Models.Enums;
using Domain.DtoClasses.Patient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.UnitOfWork;

namespace algoriza_internship_288.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public PatientController(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] AddPatientDto patientModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            bool result = await _unitOfWork.Patient.AddAsync(patientModel);
            return Ok(result);
        }

    }
}

