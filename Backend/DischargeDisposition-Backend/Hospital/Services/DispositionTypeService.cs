using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using DischargeDisposition_Backend.Infrastructure.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class DispositionTypeService : IDispositionTypeService
    {
        private readonly IDispositionTypeRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly ILogger<DispositionTypeService> _logger;

        public DispositionTypeService(
            IDispositionTypeRepository repository,
            IMemoryCache cache,
            ILogger<DispositionTypeService> logger)
        {
            _repository = repository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<ApiResponse<List<DispositionTypeResponse>>> GetAllAsync()
        {
            try
            {
                var cacheKey = CacheKeys.DispositionTypes;

                if (!_cache.TryGetValue(cacheKey, out List<DispositionTypeResponse>? result))
                {
                    _logger.LogInformation(
                        "Cache Miss -> Loading Disposition Types from Database");

                    var dispositionTypes = await _repository.GetAllAsync();

                    result = dispositionTypes
                        .Select(x => new DispositionTypeResponse
                        {
                            DispositionTypeId = x.DispositionTypeId,
                            DispositionName = x.DispositionName
                        })
                        .ToList();

                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                        .SetPriority(CacheItemPriority.High);

                    _cache.Set(cacheKey, result, cacheOptions);

                    _logger.LogInformation(
                        "Disposition Types cached successfully.");
                }
                else
                {
                    _logger.LogInformation(
                        "Cache Hit -> Disposition Types served from Memory");
                }

                if (!result.Any())
                {
                    return new ApiResponse<List<DispositionTypeResponse>>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message = "No disposition types found"
                    };
                }

                return new ApiResponse<List<DispositionTypeResponse>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Disposition types retrieved successfully",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error retrieving Disposition Types.");

                return new ApiResponse<List<DispositionTypeResponse>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve disposition types",
                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }
    }
}