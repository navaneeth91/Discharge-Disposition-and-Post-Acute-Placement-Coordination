using DischargeDisposition_Backend.Hospital.Models;

namespace DischargeDisposition_Backend.Hospital.Repositories.Interfaces
{
    public interface IAuthorizationRepository
    {
        Task<AuthorizationTracking?> GetAsync(long id);

        Task<List<AuthorizationTracking>>GetByPatientAsync(int patientId);

        Task<AuthorizationTracking?>GetByExternalIdAsync(string externalId);

        Task<AuthorizationTracking?>
            GetByInsuranceRequestIdAsync(
                int authorizationRequestId);

        Task AddAsync(
            AuthorizationTracking authorization);

        Task SaveAsync();
    }
}