using AutoMapper;
using HealthCareApi_dev_v3.Models;
using HealthCareApi_dev_v3.Models.DTO;
using HealthCareApi_dev_v3.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApi_dev_v3.Repositories
{
    public class OfficeRepository : IOfficeRepository
    {
        HealthcareContext Context { get; set; }
        public async Task<Office> GetOfficeById(Guid officeId)
        {
            var existingOffice = await Context.Office
                .Include(o => o.OfficeSpeciality)
                     .ThenInclude(os => os.Speciality)
                .FirstOrDefaultAsync(o => o.Id == officeId);

            return existingOffice;
            
        }

    }
}
