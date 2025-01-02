using HealthCareApi_dev_v3.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareApi_dev_v3.Repositories
{
    public interface ISpecialityRepository
    {
        public Task<IEnumerable<Speciality>> GetSpecialities();

        public Task<Speciality> GetSpecialityByName(string name);

        public Task<Speciality> CreateSpeciality(Speciality newSpeciality);

        public Task<Response> DeleteSpeciality(string name);

        public Task<Response> EnableSpeciality(string name);

    }
}
