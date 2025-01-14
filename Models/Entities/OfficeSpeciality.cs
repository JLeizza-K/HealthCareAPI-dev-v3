namespace HealthCareApi_dev_v3.Models.Entities
{
    public class OfficeSpeciality
    {
        public Guid Id { get; set; }
        //Si no le pongo ID, me tira error EF diciendo que tengo que aclararle en OnModelCreating que no va a tener key. 
        //¿Porqué lo hace acá y no en PractitionerSpeciality?
        public Guid OfficeId { get; set; }
        public Guid SpecialityId { get; set; }

        //Navigation properties.
        public Office office { get; set; }
        public Speciality speciality { get; set; }
    }
}
