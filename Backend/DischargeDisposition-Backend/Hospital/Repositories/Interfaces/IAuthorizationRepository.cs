using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Models;

namespace DischargeDisposition_Backend.Hospital.Repositories.Interfaces
{
    public interface IAuthorizationRepository
    {
        Task<AuthorizationTracking?> GetAsync(long id);

        Task<List<AuthorizationTracking>>GetByPatientAsync(int patientId);

        Task<AuthorizationTracking?>GetByExternalIdAsync(string externalId);
        Task<HospitalPagedResponse<AuthorizationTrackingResponseDto>> GetByCareManagerIdAsync(
           int careManagerId,
           int page,
           int pageSize,
           string? search = null,
           AuthorizationStatus? status = null,
           CancellationToken cancellationToken = default);

        Task<AuthorizationTracking?>
            GetByInsuranceRequestIdAsync(
                int authorizationRequestId);

        Task AddAsync(
            AuthorizationTracking authorization);

        Task SaveAsync();
    }
}