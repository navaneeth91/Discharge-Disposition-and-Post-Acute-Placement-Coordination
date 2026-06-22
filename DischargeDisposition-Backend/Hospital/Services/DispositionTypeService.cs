using DischargeDisposition_Backend.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class DispositionTypeService : IDispositionTypeService
    {
        private readonly IDispositionTypeRepository _repository;

        public DispositionTypeService(
            IDispositionTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<List<DispositionTypeResponse>>> GetAllAsync()
        {
            var dispositionTypes =
                await _repository.GetAllAsync();

            var result = dispositionTypes
                .Select(x => new DispositionTypeResponse
                {
                    DispositionTypeId = x.DispositionTypeId,
                    DispositionName = x.DispositionName
                })
                .ToList();

            return new ApiResponse<List<DispositionTypeResponse>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Disposition types retrieved successfully",
                Data = result
            };
        }
    }
}