using AutoMapper;
using HealthCareApi_dev_v3.Models;
using HealthCareApi_dev_v3.Models.DTO;
using HealthCareApi_dev_v3.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Concurrent;
using System.Reflection.Metadata.Ecma335;


namespace HealthCareApi_dev_v3.Repositories
{
    public class PractitionerRepository:IPractitionerRepository
    {
        public HealthcareContext Context { get; set; }
        public IMapper Mapper { get; set; }
        public ISpecialityRepository SpecialityRepository { get; set; }


        public PractitionerRepository(HealthcareContext context, IMapper mapper, ISpecialityRepository specialityRepository)
        {
            Context = context;
            Mapper = mapper;
            SpecialityRepository = specialityRepository;

        }             
       
       public async Task<IEnumerable<PractitionerDTO>> GetAll()
        {
            var practitioners = await Context.Practitioner.ToListAsync();

            //Select hace que los elementos de una secuencia puedan tener otra forma. Le pido que retorne practitioners, pero mapeando cada practitioner a PractitionerDTO.
            return practitioners.Select(practitioner => Mapper.Map<PractitionerDTO>(practitioner));
        }

        public async Task<Practitioner> GetById(Guid id)
        {
            var practitioner = await Context.Practitioner
                .Include(p => p.PractitionerSpeciality)
                    .ThenInclude(ps => ps.Speciality)
                .FirstOrDefaultAsync(p => (p.Id) == id);

            return practitioner;
        }

        public async Task<Practitioner> GetByEmail(string email)
        {
            var practitioner = await Context.Practitioner
                .Include(p => p.PractitionerSpeciality)
                    .ThenInclude(ps => ps.Speciality)
                .FirstOrDefaultAsync(p => (p.Email) == email);

            return practitioner;
        }

        public async Task<Response> CreatePractitioner(PractitionerCreateDTO practitioner)
        {
            var newPractitioner = Mapper.Map<Practitioner>(practitioner);

            newPractitioner.Id = Guid.NewGuid();
            
            await Context.Practitioner.AddAsync(newPractitioner);

            var practitionerSpeciality = new List<PractitionerSpeciality>();

            foreach (var speciality in practitioner.Speciality)
            {
                var existingSpeciality = await SpecialityRepository.GetByName(speciality.Name);

                practitionerSpeciality.Add(new PractitionerSpeciality { PractitionerId = newPractitioner.Id, SpecialityId = existingSpeciality.Id });

            }

            Context.SaveChanges();

            return new Response { Code = 200, Message = "Practitioner created successfully" };
        }

        public async Task<Response> UpdatePractitioner(PractitionerUpdateDTO practitioner)
        {
            var existingPractitioner = await GetByEmail(practitioner.Email);
            if (existingPractitioner == null)
            {
                return new Response { Code = 404, Message = "Practitioner not found" };
            };

            Mapper.Map(practitioner, existingPractitioner);
       
            Context.Practitioner.Update(existingPractitioner);

            await Context.SaveChangesAsync();

            return new Response { Code = 200, Message = "Practitioner updated successfully" };

        }

        public async Task<Response> DeletePractitioner(Guid id)
        {
            var existingPractitioner = await GetById(id);
         
            existingPractitioner.IsActive = false;

            await Context.SaveChangesAsync();

            return new Response { Code = 200, Message = "Practitioner deleted successfully" };
        }

        
        public async Task<Response> CreateAvailability (AvailabilityCreateDTO payload)
        {
            var availability = Mapper.Map<Availability>(payload);

            var existingPractSpec = Context.PractitionerSpeciality.
                Any(ps => ps.PractitionerId == payload.PractitionerId && ps.SpecialityId == payload.SpecialityId);

            if (!existingPractSpec)
            {
                return new Response { Code = 409, Message = "The selected practitioner does not practice the requested specialty." };
            }

            availability.Practitioner = Context.Practitioner.FirstOrDefault(p => p.Id == payload.PractitionerId);
            availability.Speciality = Context.Speciality.FirstOrDefault(s => s.Id == payload.SpecialityId);
            availability.Office = AssignOffice(payload, payload.SpecialityId);

            if (availability.Office == null)
            {
                var generalSpecialityId = Context.Speciality.FirstOrDefault(s => s.Name == "General").Id;

                if (generalSpecialityId != null)
                {
                    availability.Office = AssignOffice(payload, generalSpecialityId);
                }
            }

            if (availability.Office == null)
                return new Response { Code = 409, Message = "Couldn't find an available office, please try another time range or try again later." };

            availability.OfficeId = availability.Office.Id;
            availability.TimeSlot = CreateTimeSlots(payload);
            availability.PractitionerId = availability.Practitioner.Id;
            availability.SpecialityId = availability.Speciality.Id;

            Context.Add(availability);

            if (await Context.SaveChangesAsync() != 0)
                return new Response { Code = 200, Message = "Availability created successfully" };
            else
                return new Response { Code = 200 };
        }

        private Office AssignOffice(AvailabilityCreateDTO payload, Guid SpecialityId)
        {
            var offices = Context.Office.Where(o => o.OfficeSpeciality.Any(os => os.SpecialityId == SpecialityId))
                            .ToList();

            if (offices.Count == 0)
            {
                return null;

            }
            else
            {

                foreach (Office office in offices)
                {

                    var officeAvailable = !Context.Availability.Any(a => a.Office.Id == office.Id &&
                    a.StartAvailability.CompareTo(payload.FinishAvailability) < 0 &&
                    a.FinishAvailability.CompareTo(payload.StartAvailability) > 0);

                    if (officeAvailable)
                    {
                        return office;
                    }
                }

            }
            return null;
        }

        private List<TimeSlot> CreateTimeSlots(AvailabilityCreateDTO payload)
        {
            var currentTime = payload.StartAvailability;

            List<TimeSlot> slots = new List<TimeSlot>();

            while (currentTime <= payload.FinishAvailability.AddMinutes(-payload.AppointmentLenght))
            {
               currentTime = currentTime.AddMinutes(payload.AppointmentLenght);

                var timeslot = new TimeSlot { Id = Guid.NewGuid(), Status = "Available", Time = currentTime };

                slots.Add(timeslot);

                Context.Add(timeslot);
            }

            return slots;
        }

    }
}
