namespace HealthCareApi_dev_v3.Models.Entities
{
    public class PractitionerSpeciality
    {
        public Guid PractitionerId { get; set; }
        public Guid SpecialityId { get; set; }
        //Estas son las navigation properties. Permiten navegar y acceder a las entidades relacionadas.
        public Practitioner Practitioner { get; set; }
        public Speciality Speciality { get; set; }

    }
}
