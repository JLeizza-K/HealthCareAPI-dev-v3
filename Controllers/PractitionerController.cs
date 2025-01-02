using AutoMapper;
using HealthCareApi_dev_v3.Models.DTO;
using HealthCareApi_dev_v3.Models.Entities;
using HealthCareApi_dev_v3.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareApi_dev_v3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PractitionerController : ControllerBase
    {
        public IPractitionerRepository Repository;
        public PractitionerController(IPractitionerRepository repository)
        {
            Repository = repository;   
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PractitionerDTO>>> GetAll()
        {
            IEnumerable<PractitionerDTO> result = await Repository.GetAll();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePractitioner(Practitioner practitioner)
        {
            var existingPractitioner = await Repository.GetById(practitioner.Id);
           
            if (existingPractitioner != null || practitioner == null || practitioner.Id == Guid.Empty)
            {
                return BadRequest();
            }
           
            var newPractitioner = await Repository.CreatePractitioner(practitioner);
            
            return Ok(newPractitioner);
        }

        [HttpPatch]
        public async Task<ActionResult> UpdatePractitioner (PractitionerUpdateDTO practitioner)
        {
            if (practitioner.Id == Guid.Empty)
            {
                return BadRequest();
            }
             //Hago dos veces el get by id, acá y dentro del repository, no sería mejor validarlo dentro del repository y no tengo que hacer dos veces la consulta?
            var existingPractitioner = await Repository.GetById(practitioner.Id);
            if (existingPractitioner == null)
            {
                return BadRequest();
            }
            else
            {
                await Repository.UpdatePractitioner(practitioner);
                return Ok(practitioner);
            }

        }


    }
}
