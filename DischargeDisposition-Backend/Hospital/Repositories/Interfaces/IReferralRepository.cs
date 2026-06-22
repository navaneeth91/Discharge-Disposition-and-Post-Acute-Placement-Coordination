using DischargeDisposition_Backend.Hospital.Models;

namespace DischargeDisposition_Backend.Hospital.Repositories.Interfaces
{
    public interface IReferralRepository
    {
        Task<IEnumerable<Referral>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Referral?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Referral> CreateAsync(Referral referral, CancellationToken cancellationToken = default);
        Task UpdateAsync(Referral referral, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Referral>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Referral>> GetByProviderIdAsync(int providerId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Referral>> GetPendingReferralsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Referral>> GetCompletedReferralsAsync(CancellationToken cancellationToken = default);
       
    }
}