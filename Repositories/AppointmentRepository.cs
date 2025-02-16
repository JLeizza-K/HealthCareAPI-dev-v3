using AutoMapper;
using HealthCareApi_dev_v3.Models;
using HealthCareApi_dev_v3.Models.DTO;
using HealthCareApi_dev_v3.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApi_dev_v3.Repositories
{
    public class AppointmentRepository :IAppointmentRepository
    {
        HealthcareContext Context { get; set; }
        public IMapper Mapper { get; set; }
        public AppointmentRepository(IMapper mapper, HealthcareContext context)
        {
            Context = context;
            Mapper = mapper;

        }
        public async Task<Response> CreateAppointment(AppointmentDTO payload)
        {
            var existingPatient = await Context.Patient.FirstOrDefaultAsync(p => p.Id == payload.PatientId);
            if (existingPatient == null)
            {
                return new Response { Code = 404, Message = "Patient doesn't exist" };
            }

            var existingTimeSlot = await Context.TimeSlot.FirstOrDefaultAsync(t => t.Id == payload.TimeSlotId);
            if (existingTimeSlot == null)
            {
                return new Response { Code = 404, Message = "TimeSlot doesn't exist" };
            }

            var appointment = Mapper.Map<Appointment>(payload);
            Context.Add(appointment);
            Context.SaveChanges();


            return new Response { Code = 200, Message = "Appointment created successfully" };

        }
    }
}
