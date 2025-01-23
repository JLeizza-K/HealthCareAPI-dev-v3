using AutoMapper;
using HealthCareApi_dev_v3.Models;
using HealthCareApi_dev_v3.Models.DTO;
using HealthCareApi_dev_v3.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace HealthCareApi_dev_v3.Repositories
{
    public class SpecialityRepository : ISpecialityRepository
    {
        public HealthcareContext Context { get; set; }
        public IMapper Mapper { get; set; } 

        public SpecialityRepository(HealthcareContext context, IMapper mapper) 
        {
            Context = context;
            Mapper = mapper;
        }

        public async Task<IEnumerable<SpecialityDTO>> GetSpecialities()
        {
            var specialities = await Context.Speciality
                .ToListAsync();

            return specialities
                .Where(speciality => speciality.Enable)
                .Select(speciality => Mapper.Map<SpecialityDTO>(speciality));
        }

        public async Task<SpecialityDTO> GetByName(string name)
        {
            var existingSpeciality = await Context.Speciality
                .Include(s => s.PractitionerSpeciality)
                    .ThenInclude(ps => ps.Practitioner)
                 .Include(s => s.OfficeSpeciality)
                    .ThenInclude(os => os.Office)
                .FirstOrDefaultAsync(x => x.Name == name);

            return Mapper.Map<SpecialityDTO>(existingSpeciality);
        }
        public async Task<Speciality> GetById(Guid specialityId)
        {
            var speciality = await Context.Speciality
               /* .Include(s => s.PractitionerSpeciality)
                    .ThenInclude(ps => ps.Practitioner)
                 .Include(s => s.OfficeSpeciality)
                    .ThenInclude(os => os.Office) */
                    .FirstOrDefaultAsync(x => x.Id == specialityId);

            return speciality;
        }
        public async Task<SpecialityDTO> CreateSpeciality(SpecialityDTO speciality)
        {
            var newSpeciality = Mapper.Map<Speciality>(speciality);

            newSpeciality.Id = Guid.NewGuid();

            await Context.Speciality.AddAsync(newSpeciality);

            Context.SaveChanges();
            
            return speciality;
        }

        public async Task<Response> DeleteSpeciality(Guid id)
        {
            var existingSpeciality = await GetById(id);
           
            existingSpeciality.Enable = false;
            
            Context.Update(existingSpeciality);
            
            Context.SaveChanges();
           
            return new Response() { Code = 200, Message = "Speciality deleted" };
        }

        public async Task<Response> EnableSpeciality(Guid id)
        {
            var existingSpeciality = await GetById(id);
           
            existingSpeciality.Enable = true;
            
            Context.Update(existingSpeciality);
            
            Context.SaveChanges();

            return new Response() { Code = 200, Message = "Speciality enabled" };
        }
       

    }
}
