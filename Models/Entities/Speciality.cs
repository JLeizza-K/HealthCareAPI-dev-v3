namespace HealthCareApi_dev_v3.Models.Entities
{
    public class Speciality
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; }

        //Estas son las navigation properties. Permiten navegar y acceder a las entidades relacionadas.
        public IEnumerable<PractitionerSpeciality> PractitionerSpeciality { get; set; }
        public IEnumerable<Availability> Availabilities { get; set; }
        public IEnumerable<OfficeSpeciality> OfficeSpeciality { get; set; }
    }
}
