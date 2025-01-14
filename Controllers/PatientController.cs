using AutoMapper;
using HealthCareApi_dev_v3.Models.DTO;
using HealthCareApi_dev_v3.Models.Entities;
using HealthCareApi_dev_v3.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareApi_dev_v3.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        public IPatientRepository Repository { get; set; }

        public PatientController (IPatientRepository patientRepository)
        {
            Repository = patientRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetAll()
        {
            IEnumerable<PatientDTO> result = await Repository.GetPatients();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePatient(PatientCreateDTO patient)
        {
            var existingPatient = await Repository.GetByEmail(patient.Email);

            if (existingPatient != null || patient == null)
            {
                return BadRequest();
            }

            var newPatient = await Repository.CreatePatient(patient);

            return Ok(newPatient);
        }

        [HttpPatch]
        public async Task<ActionResult> UpdatePatient(PatientUpdateDTO patient)
        {
            //Hago dos veces el get by id, acá y dentro del repository, no sería mejor validarlo dentro del repository y no tengo que hacer dos veces la consulta?
            //Haciendo los updates con el email, no podría actualizarlo nunca. ¿Lo puedo hacer de otra manera?
            var existingPatient = await Repository.GetByEmail(patient.Email);
            if (existingPatient == null)
            {
                return BadRequest();
            }
            else
            {
                await Repository.UpdatePatient(patient);
                return Ok(patient);
            }

        }

        [HttpDelete]

        public async Task<ActionResult> DeletePatient (Guid id)
        {
            var existingPatient = await Repository.GetById(id);
            if (existingPatient == null)
            {
                return BadRequest();
            }
            else
            {
                await Repository.DeletePatient(id);
                return Ok();
            }
        }

    }
}
