using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface IReferralService
    {
        Task<IEnumerable<ReferralResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ReferralResponseDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<ReferralResponseDto> CreateAsync(CreateReferralDto dto, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(int id, UpdateReferralDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ReferralResponseDto>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ReferralResponseDto>> GetByProviderIdAsync(int providerId, CancellationToken cancellationToken = default);
        Task<IEnumerable<ReferralResponseDto>> GetPendingReferralsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<ReferralResponseDto>> GetCompletedReferralsAsync(CancellationToken cancellationToken = default);
    }
}