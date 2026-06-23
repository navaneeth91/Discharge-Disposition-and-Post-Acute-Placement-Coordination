using DischargeDisposition_Backend.Hospital.Models;

namespace DischargeDisposition_Backend.Hospital.Repositories.Interfaces
{
    public interface IPostAcuteProviderRepository
    {
        Task<List<PostAcuteProvider>> GetAllAsync();
        Task<PostAcuteProvider?> GetByIdAsync(int providerId);

        Task<List<PostAcuteProvider>>GetByDispositionTypeAsync(int dispositionTypeId);
    }
}