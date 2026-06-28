using DischargeDisposition_Backend.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using DischargeDisposition_Backend.Infrastructure.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class DelayReasonCodeService : IDelayReasonCodeService
    {
        private readonly IDelayReasonCodeRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly ILogger<DelayReasonCodeService> _logger;

        public DelayReasonCodeService(
            IDelayReasonCodeRepository repository,
            IMemoryCache cache,
            ILogger<DelayReasonCodeService> logger)
        {
            _repository = repository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<ApiResponse<List<DelayReasonCodeResponse>>> GetAllAsync()
        {
            try
            {
                var cacheKey = CacheKeys.DelayReasonCodes;

                if (!_cache.TryGetValue(cacheKey, out List<DelayReasonCode>? reasons))
                {
                    _logger.LogInformation(
                        "Cache Miss -> Loading Delay Reason Codes from Database");

                    reasons = (await _repository.GetAllAsync()).ToList();

                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                        .SetPriority(CacheItemPriority.High);

                    _cache.Set(cacheKey, reasons, cacheOptions);

                    _logger.LogInformation(
                        "Delay Reason Codes cached successfully.");
                }
                else
                {
                    _logger.LogInformation(
                        "Cache Hit -> Delay Reason Codes served from Memory");
                }

                var result = reasons
                    .Select(x => new DelayReasonCodeResponse
                    {
                        Id = x.Id,
                        ReasonName = x.ReasonName
                    })
                    .ToList();

                if (!result.Any())
                {
                    return new ApiResponse<List<DelayReasonCodeResponse>>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message = "No delay reason codes found"
                    };
                }

                return new ApiResponse<List<DelayReasonCodeResponse>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Delay reasons retrieved successfully",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error retrieving Delay Reason Codes.");

                return new ApiResponse<List<DelayReasonCodeResponse>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve delay reasons",
                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }
    }
}