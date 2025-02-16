namespace HealthCareApi_dev_v3.Models.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid TimeSlotId { get; set; }
        public DateTime Created { get; set; }

        //Navigation properties.
        public Patient Patient { get; set; }
        public TimeSlot TimeSlot { get; set; }

        

    }
}
