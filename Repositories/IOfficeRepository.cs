using HealthCareApi_dev_v3.Models.DTO;
using HealthCareApi_dev_v3.Models.Entities;

namespace HealthCareApi_dev_v3.Repositories
{
    public interface IOfficeRepository
    {
        public  Task<Office> GetOfficeById(Guid officeId);
    }
}
