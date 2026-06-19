using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface ILengthOfStayService
    {
        
            Task<LOSResponseDto?> GetPatientLOSAsync(int patientId);

            Task<List<LOSResponseDto>> GetAllLOSAsync();
        
    }
}
