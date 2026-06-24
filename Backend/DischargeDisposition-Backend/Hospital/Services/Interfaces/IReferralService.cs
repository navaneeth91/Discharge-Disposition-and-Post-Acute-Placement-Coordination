using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Enums;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface IReferralService
    {
        Task<ApiResponse<List<ReferralResponseDto>>>
            GetAllAsync(
                CancellationToken cancellationToken = default);

        Task<ApiResponse<ReferralResponseDto>>
            GetByIdAsync(
                int id,
                CancellationToken cancellationToken = default);

        Task<ApiResponse<ReferralResponseDto>>
            CreateAsync(
                CreateReferralDto dto,
                CancellationToken cancellationToken = default);

        Task<ApiResponse<object>>
            UpdateAsync(
                int id,
                UpdateReferralDto dto,
                CancellationToken cancellationToken = default);

        Task<ApiResponse<object>>
            DeleteAsync(
                int id,
                CancellationToken cancellationToken = default);

        Task<ApiResponse<List<ReferralResponseDto>>>
            GetByPatientIdAsync(
                int patientId,
                CancellationToken cancellationToken = default);

        Task<ApiResponse<List<ReferralResponseDto>>>
            GetByProviderIdAsync(
                int providerId,
                CancellationToken cancellationToken = default);

        Task<ApiResponse<List<ReferralResponseDto>>>
            GetPendingReferralsAsync(
                CancellationToken cancellationToken = default);

        Task<ApiResponse<List<ReferralResponseDto>>>
            GetCompletedReferralsAsync(
                CancellationToken cancellationToken = default);

        Task<ApiResponse<object>>
            UpdateStatusAsync(
                int referralId,
                AuthorizationStatus status,
                CancellationToken cancellationToken);
    }
}