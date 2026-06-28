using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using DischargeDisposition_Backend.Infrastructure.Caching;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;
        private readonly ILogger<DashboardService> _logger;
        private readonly IMemoryCache _cache;

        public DashboardService(
            IDashboardRepository repository,
            ILogger<DashboardService> logger,
            IMemoryCache cache)
        {
            _repository = repository;
            _logger = logger;
            _cache = cache;
        }

        public async Task<ApiResponse<HospitalDashboard>> GetHospitalDashboardAsync()
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var cacheKey = CacheKeys.HospitalDashboard;

                if (!_cache.TryGetValue(cacheKey, out HospitalDashboard? dashboard))
                {
                    _logger.LogInformation(
                        "Cache Miss -> Loading Hospital Dashboard from Database");

                    dashboard = await _repository.GetHospitalDashboardAsync();

                    if (dashboard != null)
                    {
                        var cacheOptions = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(2))
                            .SetSlidingExpiration(TimeSpan.FromMinutes(1))
                            .SetPriority(CacheItemPriority.High);

                        _cache.Set(cacheKey, dashboard, cacheOptions);

                        _logger.LogInformation(
                            "Hospital Dashboard cached successfully.");
                    }
                }
                else
                {
                    _logger.LogInformation(
                        "Cache Hit -> Hospital Dashboard served from Memory");
                }

                stopwatch.Stop();

                _logger.LogInformation(
                    "Performance | Service: DashboardService | Method: GetHospitalDashboardAsync | Execution Time: {ElapsedMilliseconds} ms",
                    stopwatch.ElapsedMilliseconds);

                if (stopwatch.ElapsedMilliseconds > 1000)
                {
                    _logger.LogWarning(
                        "Performance Warning | GetHospitalDashboardAsync took {ElapsedMilliseconds} ms",
                        stopwatch.ElapsedMilliseconds);
                }

                if (dashboard == null)
                {
                    return new ApiResponse<HospitalDashboard>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message = "Dashboard data not found"
                    };
                }

                return new ApiResponse<HospitalDashboard>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Dashboard retrieved successfully",
                    Data = dashboard
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogInformation(
                    "Performance | Service: DashboardService | Method: GetHospitalDashboardAsync | Failed after {ElapsedMilliseconds} ms",
                    stopwatch.ElapsedMilliseconds);

                _logger.LogError(
                    ex,
                    "Error retrieving Hospital Dashboard.");

                return new ApiResponse<HospitalDashboard>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve dashboard",
                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<List<PatientDistribution>>> GetPatientDistributionAsync()
        {
            try
            {
                var cacheKey = "PatientDistribution";

                if (!_cache.TryGetValue(cacheKey, out List<PatientDistribution>? patients))
                {
                    _logger.LogInformation(
                        "Cache Miss -> Loading Patient Distribution from Database");

                    patients = await _repository.GetPatientDistributionAsync();

                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(2))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1))
                        .SetPriority(CacheItemPriority.High);

                    _cache.Set(cacheKey, patients, cacheOptions);
                }
                else
                {
                    _logger.LogInformation(
                        "Cache Hit -> Patient Distribution served from Memory");
                }

                return new ApiResponse<List<PatientDistribution>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Patient distribution retrieved successfully",
                    Data = patients
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Patient Distribution.");

                return new ApiResponse<List<PatientDistribution>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve patient distribution",
                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<List<AuthorizationAnalytics>>> GetAuthorizationAnalyticsAsync()
        {
            try
            {
                var cacheKey = "AuthorizationAnalytics";

                if (!_cache.TryGetValue(cacheKey, out List<AuthorizationAnalytics>? analytics))
                {
                    _logger.LogInformation(
                        "Cache Miss -> Loading Authorization Analytics from Database");

                    analytics = await _repository.GetAuthorizationAnalyticsAsync();

                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(2))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(1))
                        .SetPriority(CacheItemPriority.High);

                    _cache.Set(cacheKey, analytics, cacheOptions);
                }
                else
                {
                    _logger.LogInformation(
                        "Cache Hit -> Authorization Analytics served from Memory");
                }

                return new ApiResponse<List<AuthorizationAnalytics>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Authorization analytics retrieved successfully",
                    Data = analytics
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Authorization Analytics.");

                return new ApiResponse<List<AuthorizationAnalytics>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve authorization analytics",
                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }
    }
}