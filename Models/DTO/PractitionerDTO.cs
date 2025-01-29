namespace HealthCareApi_dev_v3.Models.DTO
{
    public class PractitionerDTO
    {
       
        public string Name { get; set; }
        public string LastName { get; set; }
        public int LicenceNumber { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<SpecialityDTO> Speciality { get; set; }



    }
}
