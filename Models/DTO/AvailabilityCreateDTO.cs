using HealthCareApi_dev_v3.Models.Entities;

namespace HealthCareApi_dev_v3.Models.DTO
{
    public class AvailabilityCreateDTO
    {
        public Guid PractitionerId { get; set; }
        public Guid SpecialityId { get; set; }

        public DateTime StartAvailability { get; set; }
        public DateTime FinishAvailability { get; set; }
        public int AppointmentLenght { get; set; }


    }
}
