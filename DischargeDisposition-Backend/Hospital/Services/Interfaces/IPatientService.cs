using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface IPatientService
    {
        IEnumerable<PatientResponseDto> GetPatients();
    }
}
