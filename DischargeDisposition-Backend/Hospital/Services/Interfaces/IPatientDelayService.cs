using DischargeDisposition_Backend.DTOs.Requests;
using DischargeDisposition_Backend.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface IPatientDelayService
    {
        Task<ApiResponse<PatientDelayResponse>>
            CreateAsync(
                CreatePatientDelayRequest request);

        Task<ApiResponse<List<PatientDelayDetailsResponse>>>
    GetByPatientIdAsync(int patientId);
    }
}