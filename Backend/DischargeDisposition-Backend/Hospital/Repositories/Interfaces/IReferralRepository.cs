using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.Models;

namespace DischargeDisposition_Backend.Hospital.Repositories.Interfaces
{
    public interface IReferralRepository
    {
        Task<Referral?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Referral> CreateAsync(Referral referral, CancellationToken cancellationToken = default);
        Task UpdateAsync(Referral referral, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Referral>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Referral>> GetByProviderIdAsync(int userId, ProviderReferralQueryDto query,CancellationToken cancellationToken = default);
       
        Task<IEnumerable<Referral>> GetPendingReferralsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Referral>> GetCompletedReferralsAsync(CancellationToken cancellationToken = default);

        Task<ReferralDetailsDto?> GetReferralDetailsAsync(int UserId,int referralId);

        Task<ProviderDashboardDto> GetDashboardSummaryAsync(int userId);
        Task<HospitalPagedResponse<ReferralResponseDto>> GetByCareManagerIdAsync(
            int careManagerId,
            int page,
            int pageSize,
            string? search = null,
            AuthorizationStatus? status = null,
            CancellationToken cancellationToken = default);
        
        Task<PagedResult<Referral>> GetAllAsync(
            int page,
            int pageSize,
            string? search,
            string? status);
        Task<IEnumerable<Referral>> GetPendingByProviderIdAsync(int userId, CancellationToken cancellationToken = default);
    }
}