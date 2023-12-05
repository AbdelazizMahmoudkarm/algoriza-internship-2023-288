﻿using algoriza_internship_288.Core.Models;
using algoriza_internship_288.Core.Models.Enums;
using Domain.DtoClasses;
using Domain.DtoClasses.Appointment;
using Repository.Repository;

namespace Repository.IRepository
{
    public  interface IAppointmentRepository
    {
        // public int? GetBy(Days day,int doctorId);
        public List<Appointment> AddAppointments(List<AddAppointmentDto> model, int doctorId);
        public IQueryable<GetAppointmentDto> GetDaysAndTimes(int doctorid);
        public dynamic GetAppointmentIdWithTimeIdOrDefault(int doctorId, Days day, double time);
        public bool UpdateAppointment(EditAppointmentDto appointment);
        public bool DeleteAppointment(int hourId);
    }
}