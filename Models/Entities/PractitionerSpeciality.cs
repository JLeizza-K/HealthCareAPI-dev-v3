namespace HealthCareApi_dev_v3.Models.Entities
{
    public class PractitionerSpeciality
    {
        public Guid Id { get; set; }
        public Guid PractitionerId { get; set; }
        public Guid SpecialityId { get; set; }
        public Practitioner Practitioner { get; set; }
        public Speciality Speciality { get; set; }

    }
}
