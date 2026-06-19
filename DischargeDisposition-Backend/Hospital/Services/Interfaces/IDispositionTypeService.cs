using DischargeDisposition_Backend.DTOs.Responses;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface IDispositionTypeService
    {
        Task<ApiResponse<List<DispositionTypeResponse>>> GetAllAsync();
    }
}