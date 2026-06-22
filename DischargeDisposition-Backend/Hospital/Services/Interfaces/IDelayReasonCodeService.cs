using DischargeDisposition_Backend.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface IDelayReasonCodeService
    {
        Task<ApiResponse<List<DelayReasonCodeResponse>>>
            GetAllAsync();
    }
}