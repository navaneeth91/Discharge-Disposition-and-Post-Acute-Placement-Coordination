using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Services;
using DischargeDisposition_Backend.Infrastructure.Caching;
using DischargeDisposition_Backend.Insurance.DTOs.Responses;
using DischargeDisposition_Backend.Insurance.Repositories.Interfaces;
using DischargeDisposition_Backend.Insurance.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
namespace DischargeDisposition_Backend.Insurance.Services
{
    public class InsuranceDashboardService
        : IInsuranceDashboardService
    {
        private readonly
            IInsuranceDashboardRepository
            _repository;
        private readonly ILogger<InsuranceDashboardService> _logger;
        private readonly IMemoryCache _cache;

        public InsuranceDashboardService(
            IInsuranceDashboardRepository repository, ILogger<InsuranceDashboardService> logger, IMemoryCache cache)
        {
            _repository = repository;
            _logger = logger;
            _cache = cache;
        }

        public async Task<ApiResponse<InsuranceDashboard>>GetInsuranceDashboardAsync()
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var cacheKey = CacheKeys.InsuranceDashboard;

                if (!_cache.TryGetValue(cacheKey, out InsuranceDashboard? dashboard))
                {
                    _logger.LogInformation(
                        "Cache Miss -> Loading Insurance Dashboard from Database");

                    dashboard = await _repository.GetInsuranceDashboardAsync();

                    if (dashboard != null)
                    {
                        var cacheOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(2))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(1))
                            .SetPriority(CacheItemPriority.High);

                        _cache.Set(cacheKey, dashboard, cacheOptions);

                        _logger.LogInformation(
                            "Insurance Dashboard cached successfully.");
                    }
                }
                else
                {
                    _logger.LogInformation(
                        "Cache Hit -> Insurance Dashboard served from Memory");
                }


                if (dashboard == null)
                {
                    stopwatch.Stop();

                    _logger.LogInformation(
                        "Performance | Service: InsuranceDashboard | Method: GetInsuranceDashboardAsync | Execution Time: {ElapsedMilliseconds} ms",
                        stopwatch.ElapsedMilliseconds);
                    return new ApiResponse<InsuranceDashboard>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message =
                            "Insurance dashboard data not found"
                    };
                }
                stopwatch.Stop();

                _logger.LogInformation(
                    "Performance | Service: InsuranceDashboard | Method: GetInsuranceDashboardAsync | Execution Time: {ElapsedMilliseconds} ms",
                    stopwatch.ElapsedMilliseconds);

                if (stopwatch.ElapsedMilliseconds > 1000)
                {
                    _logger.LogWarning(
                        "Performance Warning | GetInsuranceDashboardAsync took {ElapsedMilliseconds} ms",
                        stopwatch.ElapsedMilliseconds);
                }
                return new ApiResponse<InsuranceDashboard>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Insurance dashboard retrieved successfully",

                    Data = dashboard
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogInformation(
                    "Performance | Service: InsuranceDashboard | Method: GetInsuranceDashboardAsync | Failed after {ElapsedMilliseconds} ms",
                    stopwatch.ElapsedMilliseconds);

                return new ApiResponse<InsuranceDashboard>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve insurance dashboard",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<List<InsuranceAnalytics>>>GetServiceAnalyticsAsync()
        {
            try
            {
                var cacheKey = "InsuranceServiceAnalytics";

                if (!_cache.TryGetValue(cacheKey, out List<InsuranceAnalytics>? analytics))
                {
                    _logger.LogInformation(
                        "Cache Miss -> Loading Insurance Service Analytics from Database");

                    analytics = await _repository.GetServiceAnalyticsAsync();

                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(2))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1))
                        .SetPriority(CacheItemPriority.High);

                    _cache.Set(cacheKey, analytics, cacheOptions);
                }
                else
                {
                    _logger.LogInformation(
                        "Cache Hit -> Insurance Service Analytics served from Memory");
                }

                return new ApiResponse<
                    List<InsuranceAnalytics>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Service analytics retrieved successfully",

                    Data = analytics
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    List<InsuranceAnalytics>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve service analytics",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<List<AuthorizationRequestListItemResponse>>> GetRecentAuthorizationRequestsAsync(int take)
        {
            try
            {
                take = take < 1 ? 5 : take;

                var cacheKey = $"RecentAuthorizationRequests_{take}";

                if (!_cache.TryGetValue(cacheKey, out List<AuthorizationRequestListItemResponse>? requests))
                {
                    _logger.LogInformation(
                        "Cache Miss -> Loading Recent Authorization Requests from Database");

                    requests = await _repository.GetRecentAuthorizationRequestsAsync(take);

                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(1))
                        .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                        .SetPriority(CacheItemPriority.Normal);

                    _cache.Set(cacheKey, requests, cacheOptions);
                }
                else
                {
                    _logger.LogInformation(
                        "Cache Hit -> Recent Authorization Requests served from Memory");
                }

                return new ApiResponse<List<AuthorizationRequestListItemResponse>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Recent authorization requests retrieved successfully",
                    Data = requests
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<AuthorizationRequestListItemResponse>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve recent authorization requests",
                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }
    }
}