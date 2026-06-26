using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.Models;

namespace DischargeDisposition_Backend.Hospital.Repositories.Interfaces
{
    public interface IPatientAssignmentRepository
    {
        Task<PatientAssignment> AssignCareManagerAsync(PatientAssignment assignment);

        Task<PagedResult<Patient>> GetUnassignedPatientsAsync(
            int page,
            int pageSize,
            string? search);

        Task<IEnumerable<PatientAssignment>> GetPatientsByCareManagerAsync(int careManagerId);

        Task<bool> IsPatientAssignedAsync(int patientId);

        Task<PatientAssignment?> GetAssignmentByPatientIdAsync(int patientId);

        Task<PatientAssignment> UpdateAssignmentAsync(PatientAssignment assignment);

        Task<IEnumerable<User>> GetAllCareManagersAsync();

        Task<bool> CareManagerExistsAsync(int careManagerId);
    }
}