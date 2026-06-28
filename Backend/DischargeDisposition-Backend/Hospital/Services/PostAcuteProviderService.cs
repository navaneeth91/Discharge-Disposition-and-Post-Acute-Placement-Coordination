using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using DischargeDisposition_Backend.Infrastructure.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class PostAcuteProviderService
        : IPostAcuteProviderService
    {
        private readonly IPostAcuteProviderRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly ILogger<PostAcuteProviderService> _logger;

        public PostAcuteProviderService(
            IPostAcuteProviderRepository repository, IMemoryCache cache, ILogger<PostAcuteProviderService> logger)
        {
            _repository = repository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<ApiResponse<List<PostAcuteProviderResponse>>> GetAllAsync()
        {
            try
            {
                var cacheKey = CacheKeys.PostAcuteProviders;

                if (!_cache.TryGetValue(cacheKey, out List<PostAcuteProviderResponse>? result))
                {
                    _logger.LogInformation(
                        "Cache Miss -> Loading Post Acute Providers from Database");

                    var providers = await _repository.GetAllAsync();

                    result = providers
                        .Select(provider => new PostAcuteProviderResponse
                        {
                            ProviderId = provider.ProviderId,
                            ProviderName = provider.ProviderName,
                            IsActive = provider.IsActive,
                            Phone = provider.Phone,
                            Email = provider.Email,
                            ContactPerson = provider.ContactPerson,
                            AddressLine = provider.AddressLine,
                            City = provider.City,
                            State = provider.State,
                            DispositionTypeId = provider.DispositionTypeId,
                            DispositionName = provider.dispositionType.DispositionName
                        })
                        .ToList();

                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                        .SetPriority(CacheItemPriority.High);

                    _cache.Set(cacheKey, result, cacheOptions);

                    _logger.LogInformation(
                        "Post Acute Providers cached successfully.");
                }
                else
                {
                    _logger.LogInformation(
                        "Cache Hit -> Post Acute Providers served from Memory");
                }

                return new ApiResponse<List<PostAcuteProviderResponse>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Providers retrieved successfully",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error retrieving Post Acute Providers.");

                return new ApiResponse<List<PostAcuteProviderResponse>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve providers",
                    Errors = new()
            {
                ex.Message
            }
                };
            }
        }

        public async Task<
            ApiResponse<PostAcuteProviderResponse>>
            GetByIdAsync(
                int providerId)
        {
            try
            {
                var provider =
                    await _repository
                        .GetByIdAsync(providerId);

                if (provider == null)
                {
                    return new ApiResponse<
                        PostAcuteProviderResponse>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message =
                            "Provider not found"
                    };
                }

                return new ApiResponse<
                    PostAcuteProviderResponse>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Provider retrieved successfully",

                    Data =
                        new PostAcuteProviderResponse
                        {
                            ProviderId =
                                provider.ProviderId,

                            ProviderName =
                                provider.ProviderName,

                            IsActive =
                                provider.IsActive,

                            Phone =
                                provider.Phone,

                            Email =
                                provider.Email,

                            ContactPerson =
                                provider.ContactPerson,

                            AddressLine =
                                provider.AddressLine,

                            City =
                                provider.City,

                            State =
                                provider.State,

                            DispositionTypeId =
                                provider.DispositionTypeId,

                            DispositionName =
                                provider.dispositionType
                                    .DispositionName
                        }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    PostAcuteProviderResponse>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve provider",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<
            ApiResponse<List<PostAcuteProviderResponse>>>
            GetByDispositionTypeAsync(
                int dispositionTypeId)
        {
            try
            {
                var providers =
                    await _repository
                        .GetByDispositionTypeAsync(
                            dispositionTypeId);

                var result =
                    providers.Select(provider =>
                        new PostAcuteProviderResponse
                        {
                            ProviderId =
                                provider.ProviderId,

                            ProviderName =
                                provider.ProviderName,

                            IsActive =
                                provider.IsActive,

                            Phone =
                                provider.Phone,

                            Email =
                                provider.Email,

                            ContactPerson =
                                provider.ContactPerson,

                            AddressLine =
                                provider.AddressLine,

                            City =
                                provider.City,

                            State =
                                provider.State,

                            DispositionTypeId =
                                provider.DispositionTypeId,

                            DispositionName =
                                provider.dispositionType
                                    .DispositionName
                        })
                    .ToList();

                return new ApiResponse<
                    List<PostAcuteProviderResponse>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Providers retrieved successfully",

                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    List<PostAcuteProviderResponse>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve providers",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }
    }
}