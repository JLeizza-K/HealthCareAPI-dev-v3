using HealthCareApi_dev_v3.Models;
using HealthCareApi_dev_v3.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApi_dev_v3.Repositories
{
    public class SpecialityRepository : ISpecialityRepository
    {
        public HealthcareContext Context { get; set; }

        public SpecialityRepository(HealthcareContext context) 
        {
            Context = context;
        }

        public async Task<IEnumerable<Speciality>> GetSpecialities()
        {
            var specialities = await Context.Speciality.ToListAsync();

            return specialities.Where(specialities => specialities.Enable);
        }

        public Task<Speciality> GetSpecialityByName(string name)
        {
            var existingSpeciality = Context.Speciality.FirstOrDefaultAsync(x => x.Name == name);

            return existingSpeciality;
        }

        public async Task<Speciality> CreateSpeciality(Speciality speciality)
        {
            await Context.Speciality.AddAsync(speciality);

            Context.SaveChanges();
            
            return speciality;
        }

        public async Task<Response> DeleteSpeciality(string name)
        {
            var existingSpeciality = await GetSpecialityByName(name);
           
            existingSpeciality.Enable = false;
            
            Context.Update(existingSpeciality);
            
            Context.SaveChanges();
           
            return new Response() { Code = 200, Message = "Speciality deleted" };
        }

        public async Task<Response> EnableSpeciality(string name)
        {
            var existingSpeciality = await GetSpecialityByName(name);
           
            existingSpeciality.Enable = true;
            
            Context.Update(existingSpeciality);
            
            Context.SaveChanges();

            return new Response() { Code = 200, Message = "Speciality enabled" };
        }
    }
}
