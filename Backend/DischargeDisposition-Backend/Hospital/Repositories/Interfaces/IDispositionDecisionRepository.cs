using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Repositories.Interfaces
{
    public interface IDispositionDecisionRepository
    {
        Task AddAsync(DispositionDecision decision);
        Task<DispositionDecision?> GetByPatientIdAsync(int patientId);
        Task<DispositionDecision?> GetByDecisionIdAsync(int decisionId);

        Task UpdateDecisionAsync(DispositionDecision decision);
        Task<List<AssignedPatientsResponse>> GetAssignedPatientsAsync(int physicianId);
    }
}