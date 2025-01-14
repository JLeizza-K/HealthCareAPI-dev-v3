using HealthCareApi_dev_v3.Models.DTO;
using HealthCareApi_dev_v3.Models.Entities;
using HealthCareApi_dev_v3.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareApi_dev_v3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialityController : Controller
    {
        public ISpecialityRepository Repository;

        public SpecialityController(ISpecialityRepository repository)
        {
            Repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Speciality>>> GetAll()
        {
            var specialities = await Repository.GetSpecialities();

            if (!specialities.Any())
            {
                return NotFound();
            }

            return Ok(specialities);
        }

        [HttpPost]
        public async Task<ActionResult<Speciality>> CreateSpeciality(SpecialityDTO speciality)
        {
            //Esto diferencia entre mayusculas y minusculas?
            var existingSpeciality = await Repository.GetByName(speciality.Name);

            if (existingSpeciality != null)
            {
                //Puedo agregarle un mensaje al badrequest?
                return BadRequest();
            }

            return Ok(await Repository.CreateSpeciality(speciality));
        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteSpeciality(Guid id)
        {
            var existingSpeciality = await Repository.GetById(id);
            if (existingSpeciality == null)
            {
                return BadRequest();
            }

            return Ok(await Repository.DeleteSpeciality(id));
        }

        [HttpPatch]
        public async Task<ActionResult<Response>> EnableSpeciality(Guid id)
        {
            var existingSpeciality = await Repository.GetById(id);
            if (existingSpeciality == null)
            {
                return BadRequest();
            }

            return Ok(await Repository.EnableSpeciality(id));
        }
    }
}
