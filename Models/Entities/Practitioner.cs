using System.ComponentModel.DataAnnotations;

namespace HealthCareApi_dev_v3.Models.Entities
{
    public class Practitioner
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int LicenceNumber { get; set; }
        public bool IsActive { get; set; }
        public string DNI { get; set; }
        public string Cuit { get; set; }
        public string CBU { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        //Estas son las navigation properties. Permiten navegar y acceder a las entidades relacionadas.
        public IEnumerable<PractitionerSpeciality> PractitionerSpeciality { get; set; }
        public IEnumerable<Appointment> Appointment { get; set; }
    }
}
