namespace HealthCareApi_dev_v3.Models.DTO
{
    public class PatientDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Dni { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Insurance { get; set; }
    }
}
