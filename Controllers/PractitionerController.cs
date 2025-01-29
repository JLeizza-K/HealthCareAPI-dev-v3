using AutoMapper;
using HealthCareApi_dev_v3.Models.DTO;
using HealthCareApi_dev_v3.Models.Entities;
using HealthCareApi_dev_v3.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

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
        public async Task<ActionResult> CreatePractitioner(PractitionerCreateDTO practitioner)
        {
            var existingPractitioner = await Repository.GetByEmail(practitioner.Email);

            //tiene sentido verificar si el practitioner es null antes de hacer la consulta?

            if (existingPractitioner != null || practitioner == null)
            {
                return BadRequest();
            }

            var newPractitioner = await Repository.CreatePractitioner(practitioner);

            return Ok(newPractitioner);
        }

        [HttpPatch]
        public async Task<ActionResult> UpdatePractitioner(PractitionerUpdateDTO practitioner)
        {
            //Hago dos veces el get by id, acá y dentro del repository, no sería mejor validarlo dentro del repository y no tengo que hacer dos veces la consulta?
            var existingPractitioner = await Repository.GetByEmail(practitioner.Email);
            if (existingPractitioner == null)
            {
                return BadRequest(new Response { Code= 404, Message = "Practitioner not found"});
            }
            else
            {
                await Repository.UpdatePractitioner(practitioner);
                return Ok(practitioner);
            }

        }

        [HttpDelete]
        public async Task<ActionResult> DeletePractitioner (Guid id)
        {
            var existingPractitioner = await Repository.GetById(id);
            if (existingPractitioner == null)
            {
                return BadRequest(new Response { Code = 404, Message = "Practitioner not found" });
            }
            var response = Repository.DeletePractitioner(id);

            return Ok(response);
        }

        [HttpPost("/availability")]
        public async Task<ActionResult> CreateAvailability (AvailabilityCreateDTO availability)
        {
            if (availability == null)
            {
                return BadRequest();

            }
           
            if (availability.AppointmentLenght < 10)
            {
                return BadRequest(new Response { Code = 409, Message = "The appointment lenght must be longer than 10 minutes." });
            }

            var newAvailability = await Repository.CreateAvailability(availability);

            return Ok(newAvailability);

        }
    }
}
