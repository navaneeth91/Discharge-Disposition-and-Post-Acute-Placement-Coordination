using DischargeDisposition_Backend.DTOs.Requests;
using DischargeDisposition_Backend.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface IDispositionDecisionService
    {
        Task<ApiResponse<DispositionDecisionResponse>> CreateAsync(CreateDispositionDecisionRequest request);

        Task<ApiResponse<DispositionDecisionDetailsResponse>> GetByPatientIdAsync(int patientId);
        Task<ApiResponse<DispositionDecisionResponse>> UpdateDecisionAsync(int decisionId,UpdateDispositionDecisionRequest request);

        Task<ApiResponse<List<AssignedPatientsResponse>>> GetAssignedPatientsAsync();
    }
}