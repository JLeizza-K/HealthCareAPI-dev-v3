namespace HealthCareApi_dev_v3.Models.Entities
{
    public class Availability
    {
        public Guid Id { get; set; }
        public Guid PractitionerId { get; set; }
        public Guid SpecialityId { get; set; }
        public Guid OfficeId { get; set; }


        public DateTime StartAvailability { get; set; }
        public DateTime FinishAvailability { get; set; }
        public IEnumerable<TimeSlot> TimeSlot { get; set; }

        //Navigation properties.
        public Practitioner Practitioner { get; set; }
        public Office Office { get; set; }
        public Speciality Speciality { get; set; }
        
    }
}
