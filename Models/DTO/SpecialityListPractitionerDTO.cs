namespace HealthCareApi_dev_v3.Models.DTO
{
    public class SpecialityListPractitionerDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public List<PractitionerDTO> Practitioner { get; set; }
    }
}
