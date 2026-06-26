using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>>
            GetPatientsAsync();

        Task<Patient?>
            GetByIdAsync(int patientId);

        Task<Patient?>
            GetTrackedByIdAsync(int patientId);

        Task UpdatePatientAsync(
            Patient patient);

        Task<List<PatientByDeptIdResponse>> GetPatientsByDeptIdAsync(int physicianId);
    }
}