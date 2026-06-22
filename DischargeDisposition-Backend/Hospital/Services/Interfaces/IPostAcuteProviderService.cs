using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface IPostAcuteProviderService
    {
        Task<List<PostAcuteProviderResponse>> GetAllAsync();
        Task<PostAcuteProviderResponse?> GetByIdAsync(int providerId);

        Task<List<PostAcuteProviderResponse>>GetByDispositionTypeAsync(int dispositionTypeId);
    }
}