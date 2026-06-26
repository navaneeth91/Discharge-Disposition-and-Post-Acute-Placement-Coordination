using DischargeDisposition_Backend.Hospital.DTOs.Responses;

public interface IPatientService
{
    Task<ApiResponse<IEnumerable<PatientResponseDto>>>
        GetPatientsAsync();

    Task<ApiResponse<PatientResponseDto>>
        GetPatientByIdAsync(
            int patientId);

    Task<ApiResponse<object>>
        DischargePatientAsync(
            int patientId,
            DateTime actualDischargeDate);
    Task<ApiResponse<List<PatientByDeptIdResponse>>> GetPatientsByDeptIdAsync(string? search);
}