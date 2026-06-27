using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface IPatientAssignmentService
    {
        Task<ApiResponse<PatientAssignmentDto>> AssignCareManagerAsync(
            AssignCareManagerRequest request);

        Task<ApiResponse<IEnumerable<PatientDto>>> GetUnassignedPatientsAsync();

        Task<ApiResponse<PagedResponse<AssignedPatientDto>>> GetPatientsByCareManagerAsync(
            int careManagerId,
            int page,
            int pageSize,
            string? search = null);

        Task<ApiResponse<IEnumerable<UserDto>>> GetAllCareManagersAsync();
    }
}