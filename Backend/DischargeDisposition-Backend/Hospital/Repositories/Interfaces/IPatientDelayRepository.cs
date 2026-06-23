using DischargeDisposition_Backend.Hospital.Models;

namespace DischargeDisposition_Backend.Hospital.Repositories.Interfaces
{
    public interface IPatientDelayRepository
    {
        Task AddAsync(PatientDelay patientDelay);
        Task<List<PatientDelay>> GetByPatientIdAsync(int patientId);
    }
}