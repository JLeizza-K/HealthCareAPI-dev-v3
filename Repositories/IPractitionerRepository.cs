using HealthCareApi_dev_v3.Models.Entities;
using HealthCareApi_dev_v3.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace HealthCareApi_dev_v3.Repositories
{
    public interface IPractitionerRepository
    {
        Task<IEnumerable<PractitionerDTO>> GetAll();
        Task<Practitioner> GetById(Guid id);
        Task<Practitioner> GetByEmail(string email);
        Task<PractitionerCreateDTO> CreatePractitioner(PractitionerCreateDTO practitioner);
        Task<PractitionerUpdateDTO> UpdatePractitioner(PractitionerUpdateDTO practitioner);
        Task<Response> CreateAvailability(AvailabilityCreateDTO availability);

    }
}
