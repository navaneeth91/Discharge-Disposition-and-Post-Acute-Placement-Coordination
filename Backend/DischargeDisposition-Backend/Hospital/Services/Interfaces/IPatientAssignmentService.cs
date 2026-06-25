using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface IPatientAssignmentService
    {
        Task<ApiResponse<PatientAssignmentDto>> AssignCareManagerAsync(
            AssignCareManagerRequest request);

        Task<ApiResponse<IEnumerable<PatientDto>>> GetUnassignedPatientsAsync();

        Task<ApiResponse<IEnumerable<PatientAssignmentDto>>> GetPatientsByCareManagerAsync(
            int careManagerId);

        Task<ApiResponse<IEnumerable<UserDto>>> GetAllCareManagersAsync();
    }
}