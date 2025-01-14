namespace HealthCareApi_dev_v3.Models.Entities
{
    public class Office
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        //Navigation properties.
        public IEnumerable<Availability> Availabilities { get; set; }
        public IEnumerable<OfficeSpeciality> OfficeSpeciality { get; set; }
    }
}
