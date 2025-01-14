namespace HealthCareApi_dev_v3.Models.Entities
{
    public class Availability
    {
        public Guid Id { get; set; }
        public Guid PractitionerId { get; set; }
        public Guid OfficeId { get; set; }
        public Guid SpecialityId { get; set; }


        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan FinishTime { get; set; }
        public IEnumerable<TimeSlot> TimeSlot { get; set; }

        //Navigation properties.
        public Practitioner Practitioner { get; set; }
        public Office Office { get; set; }
        public Speciality Speciality { get; set; }
        
    }
}
