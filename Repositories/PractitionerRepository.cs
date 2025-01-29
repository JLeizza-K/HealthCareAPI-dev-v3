using AutoMapper;
using HealthCareApi_dev_v3.Models;
using HealthCareApi_dev_v3.Models.DTO;
using HealthCareApi_dev_v3.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Concurrent;


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
       
        async Task<IEnumerable<PractitionerDTO>> IPractitionerRepository.GetAll()
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

        public async Task<PractitionerUpdateDTO> UpdatePractitioner(PractitionerUpdateDTO practitioner)
        {
            var existingPractitioner = await GetByEmail(practitioner.Email);

            Mapper.Map(practitioner, existingPractitioner);
       
            Context.Practitioner.Update(existingPractitioner);

            await Context.SaveChangesAsync();

            return Mapper.Map<PractitionerUpdateDTO>(existingPractitioner);
        }

        public async Task<Response> DeletePractitioner(Guid id)
        {
            var existingPractitioner = await GetById(id);
         
            existingPractitioner.IsActive = false;

            await Context.SaveChangesAsync();

            return new Response { Code = 200, Message = "Practitioner deleted successfully" };
        }

        
        public async Task<Response> CreateAvailability (AvailabilityCreateDTO availability)
        {
     

            var existingPractSpec = !Context.PractitionerSpeciality.
                Any(ps => ps.PractitionerId == availability.PractitionerId && ps.SpecialityId == availability.SpecialityId);
            
            if (existingPractSpec)
            {
                return new Response { Code = 409, Message = "The selected practitioner does not practice the requested specialty." };
            }
            
            List<Office> offices = Context.Office.ToList();

            //ordena la lista por la speciality del practitioner, para revisar primero si se puede en las que coinciden con la 
            //especialidad

            offices.OrderBy(o => !o.OfficeSpeciality
                    .Any(os => os.SpecialityId == availability.SpecialityId));

            var newAvailability = Mapper.Map<Availability>(availability);

            foreach (Office office in offices)
            {
                               
                var officeAvailable = !Context.Availability.Any(a => a.Office.Id == office.Id &&
                a.StartAvailability.CompareTo(availability.FinishAvailability) < 0 &&
                a.FinishAvailability.CompareTo(availability.StartAvailability) > 0);

                if (officeAvailable == true)
                {
                    newAvailability.Office = office;
                    break;
                }
            }
           
            if (newAvailability.Office == null)
            {
                return new Response { Code = 409, Message = "Couldn't find an available office, please try another time range or try again later." };
            }

            newAvailability.OfficeId = newAvailability.Office.Id;

            var timeSpanLenght = new TimeSpan(0, availability.AppointmentLenght, 0);
            var lastAppointment = availability.FinishAvailability.Subtract(timeSpanLenght);
            var currentTime = availability.StartAvailability;
            
            List<TimeSlot> slots = new List<TimeSlot>();

            while (currentTime <= lastAppointment)
            {
                var timeslot = new TimeSlot { Id = Guid.NewGuid(), Status = "Available" };

                slots.Add(timeslot);
                
                Context.Add(timeslot);

                currentTime += timeSpanLenght;
            }
            
            newAvailability.TimeSlot = slots;

            newAvailability.Practitioner = await GetById(availability.PractitionerId);
            newAvailability.PractitionerId = newAvailability.Practitioner.Id;

            newAvailability.Speciality = await SpecialityRepository.GetById(availability.SpecialityId);
            newAvailability.SpecialityId = newAvailability.Speciality.Id;

            newAvailability.Id = Guid.NewGuid();

            Context.Add(newAvailability);
            
            await Context.SaveChangesAsync();

            return new Response { Code = 200, Message = "Availability created successfully" };
        }
        

    }
}
