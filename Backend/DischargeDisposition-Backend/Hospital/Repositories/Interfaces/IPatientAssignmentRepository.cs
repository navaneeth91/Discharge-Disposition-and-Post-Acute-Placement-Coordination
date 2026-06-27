using DischargeDisposition_Backend.Hospital.DTOs.Responses;
﻿using DischargeDisposition_Backend.Helpers;
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

        Task<PagedResponse<AssignedPatientDto>> GetPatientsByCareManagerAsync(
            int careManagerId,
            int page,
            int pageSize,
            string? search = null);
        Task<bool> IsPatientAssignedAsync(int patientId);

        Task<PatientAssignment?> GetAssignmentByPatientIdAsync(int patientId);

        Task<PatientAssignment> UpdateAssignmentAsync(PatientAssignment assignment);

        Task<IEnumerable<User>> GetAllCareManagersAsync();

        Task<bool> CareManagerExistsAsync(int careManagerId);
    }
}