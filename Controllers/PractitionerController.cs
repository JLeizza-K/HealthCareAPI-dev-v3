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
        public async Task<ActionResult<IEnumerable<Practitioner>>> GetAll()
        {
            IEnumerable<Practitioner> result = await Repository.GetAll();
            return Ok(result);
        }

        [HttpPost]

        public async Task<ActionResult> CreatePractitioner(Practitioner practitioner)
        {
            var newPractitioner = await Repository.CreatePractitioner(practitioner);
            return Ok(newPractitioner);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response>> UpdatePractitioner (Guid id , Practitioner practitioner)
        {
            var response = await Repository.UpdatePractitioner(id, practitioner);

            //Este lo tuve que desglosar con chat gpt. Entiendo que es un condicional, si el code de response es = 200, responde ok, sino, badrequest.
            return response.Code == 200 ? Ok(response) : BadRequest(response);

        }


    }
}
