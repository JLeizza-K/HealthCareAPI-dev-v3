using HealthCareApi_dev_v3.Models.DTO;
using HealthCareApi_dev_v3.Models.Entities;

namespace HealthCareApi_dev_v3.Repositories
{
    public interface IPatientRepository
    {
        public Task<IEnumerable<PatientDTO>> GetPatients();

        public Task<PatientDTO> GetPatientById(Guid patientId);

        public Task<Patient> GetPatientByEmail(string email);

        public Task<Response> CreatePatient(PatientCreateDTO patient);

        public Task<Response> UpdatePatient(PatientUpdateDTO patient);

        public Task<Response> DeletePatient(Guid id);
    }
}
