using HealthCareApi_dev_v3.Models.DTO;
using HealthCareApi_dev_v3.Models.Entities;
using HealthCareApi_dev_v3.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareApi_dev_v3.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        public IAppointmentRepository Repository { get; set; }

        public AppointmentController(IAppointmentRepository appointmentRepository)
        {
           Repository = appointmentRepository;
        }


        [HttpPost]
        public async Task<ActionResult> CreateAppointment(AppointmentDTO payload)
        {
            if (payload == null)
            {
                return BadRequest();
            }

            var newAppointment = await Repository.CreateAppointment(payload);

            return Ok(newAppointment);
        }
    }
}
