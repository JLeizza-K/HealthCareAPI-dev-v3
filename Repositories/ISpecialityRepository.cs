using HealthCareApi_dev_v3.Models.DTO;
using HealthCareApi_dev_v3.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareApi_dev_v3.Repositories
{
    public interface ISpecialityRepository
    {
        public Task<IEnumerable<SpecialityDTO>> GetSpecialities();

        public Task<SpecialityDTO> GetByName(string name);

        public Task<Speciality> GetById(Guid specialityId);

        public Task<SpecialityDTO> CreateSpeciality(SpecialityDTO speciality);

        public Task<Response> DeleteSpeciality(Guid id);

        public Task<Response> EnableSpeciality(Guid id);

    }
}
