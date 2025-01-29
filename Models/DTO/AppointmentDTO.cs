using HealthCareApi_dev_v3.Models.Entities;

namespace HealthCareApi_dev_v3.Models.DTO
{
    public class AppointmentDTO
    {
        public Guid PatientId { get; set; }
        public Guid TimeSlotId { get; set; }

    }
}
