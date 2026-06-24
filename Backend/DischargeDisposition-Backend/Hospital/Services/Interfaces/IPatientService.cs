using DischargeDisposition_Backend.Hospital.DTOs.Responses;

public interface IPatientService
{
    Task<IEnumerable<PatientResponseDto>> GetPatientsAsync();

    Task<PatientResponseDto?> GetPatientByIdAsync(int patientId);



    Task<bool> DischargePatientAsync(
        int patientId,
        DateTime actualDischargeDate);
}