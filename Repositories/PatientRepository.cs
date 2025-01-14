using AutoMapper;
using HealthCareApi_dev_v3.Models;
using HealthCareApi_dev_v3.Models.DTO;
using HealthCareApi_dev_v3.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace HealthCareApi_dev_v3.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        public HealthcareContext Context { get; set; }
        public IMapper Mapper { get; set; }
        public PatientRepository(IMapper mapper, HealthcareContext context)
        {
            Context = context;
            Mapper = mapper;

        }
        public async Task<Response> CreatePatient(PatientCreateDTO patient)
        {
           var newPatient = Mapper.Map<Patient>(patient);

            newPatient.Id = Guid.NewGuid();
            newPatient.Enable = true;
            
            await Context.AddAsync(newPatient);
            Context.SaveChanges();

            return new Response { Code = 200, Message = "Patient created successfully" }; 
        }

        public async Task<PatientDTO> GetById(Guid patientId)
        {
            var patient = await Context.Patient.FirstOrDefaultAsync(p => p.Id == patientId);
            
            return Mapper.Map<PatientDTO>(patient);

        }

        public async Task<IEnumerable<PatientDTO>> GetPatients()
        {
            var patients = await Context.Patient.ToListAsync();

            return patients.Select(patients => Mapper.Map<PatientDTO>(patients));
        }
        public async Task<Patient> GetByEmail(string email)
        {
            var patient = await Context.Patient.FirstOrDefaultAsync(p => (p.Email) == email);

            return patient;
        }
        public async Task<Response> UpdatePatient(PatientUpdateDTO patient)
        {
            var existingPatient = await GetByEmail(patient.Email);
            
            Mapper.Map(patient, existingPatient);

            Context.Patient.Update(existingPatient);

            await Context.SaveChangesAsync();

            return new Response { Code = 200, Message = "Patient updated successfully" };
        }

        public async Task<Response> DeletePatient(Guid id)
        {
            var existingPatient = await Context.Patient.FirstOrDefaultAsync(Patient => Patient.Id == id);
           
            existingPatient.Enable = false;
            
            Context.Patient.Update(existingPatient);

            await Context.SaveChangesAsync();

            return new Response { Code = 200, Message = "Patient deleted successfully" };

        }
    }
}
