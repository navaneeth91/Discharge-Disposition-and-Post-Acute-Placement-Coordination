using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface IPostAcuteProviderService
    {
        Task<ApiResponse<List<PostAcuteProviderResponse>>>
            GetAllAsync();

        Task<ApiResponse<PostAcuteProviderResponse>>
            GetByIdAsync(
                int providerId);

        Task<ApiResponse<List<PostAcuteProviderResponse>>>
            GetByDispositionTypeAsync(
                int dispositionTypeId);
    }
}