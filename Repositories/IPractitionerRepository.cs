using HealthCareApi_dev_v3.Models.Entities;
using HealthCareApi_dev_v3.Models.DTO;
using Microsoft.AspNetCore.Mvc;


namespace HealthCareApi_dev_v3.Repositories
{
    public interface IPractitionerRepository
    {
        Task<IEnumerable<PractitionerDTO>> GetAll();
        Task<PractitionerDTO> GetById(Guid id);
        Task<Practitioner> CreatePractitioner(Practitioner practitioner);
        Task<Practitioner> UpdatePractitioner(PractitionerUpdateDTO practitioner);
    }
}
