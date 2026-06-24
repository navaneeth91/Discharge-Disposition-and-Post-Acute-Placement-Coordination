
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

        public async Task< ApiResponse<List<DispositionTypeResponse>>>GetAllAsync()
        {
            try
            {
                var dispositionTypes =
                    await _repository.GetAllAsync();

                var result =
                    dispositionTypes
                    .Select(x =>
                        new DispositionTypeResponse
                        {
                            DispositionTypeId =
                                x.DispositionTypeId,

                            DispositionName =
                                x.DispositionName
                        })
                    .ToList();

                if (!result.Any())
                {
                    return new ApiResponse<List<DispositionTypeResponse>>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message =
                            "No disposition types found"
                    };
                }

                return new ApiResponse<List<DispositionTypeResponse>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Disposition types retrieved successfully",

                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    List<DispositionTypeResponse>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve disposition types",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }
    }
}