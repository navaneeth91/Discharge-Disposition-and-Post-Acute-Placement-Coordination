using DischargeDisposition_Backend.Hospital.Models;


namespace DischargeDisposition_Backend.Hospital.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetPatientsAsync();
        Task<Patient?> GetByIdAsync(int patientId);
        Task UpdatePatientAsync(Patient patient);

    }
}
