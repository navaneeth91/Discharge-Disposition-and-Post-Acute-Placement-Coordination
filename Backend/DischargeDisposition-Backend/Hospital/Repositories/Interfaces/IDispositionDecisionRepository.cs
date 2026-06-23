using DischargeDisposition_Backend.Hospital.Models;

namespace DischargeDisposition_Backend.Hospital.Repositories.Interfaces
{
    public interface IDispositionDecisionRepository
    {
        Task AddAsync(DispositionDecision decision);
        Task<DispositionDecision?> GetByPatientIdAsync(int patientId);
        Task<DispositionDecision?> GetByDecisionIdAsync(int decisionId);

        Task UpdateDecisionAsync(DispositionDecision decision);
    }
}