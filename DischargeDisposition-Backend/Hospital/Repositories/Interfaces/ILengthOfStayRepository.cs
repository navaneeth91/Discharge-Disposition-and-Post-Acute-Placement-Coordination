using DischargeDisposition_Backend.Hospital.Models;

public interface ILengthOfStayRepository
{
    Task<LengthOfStayTracking?> GetLosByPatientIdAsync(int patientId);

    Task<List<LengthOfStayTracking>> GetAllLosAsync();
}