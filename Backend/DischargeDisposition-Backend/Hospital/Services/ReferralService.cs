using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Helpers;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using DischargeDisposition_Backend.Infrastructure.Caching;
using DischargeDisposition_Backend.Infrastructure.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
namespace DischargeDisposition_Backend.Hospital.Services
{
    public class ReferralService : IReferralService
    {
        private readonly IReferralRepository _repo;
        private readonly HospitalDbContext _db;
        private readonly ILogger<ReferralService> _logger;
        private readonly IMemoryCache _cache;
        private readonly NotificationService _notificationService;

        public ReferralService(IReferralRepository repo, HospitalDbContext db, ILogger<ReferralService> logger, IMemoryCache cache, NotificationService notificationService)
        {
            _repo = repo;
            _db = db;
            _logger = logger;
            _cache = cache;
            _notificationService = notificationService;
        }

        public async Task<ApiResponse<PagedResult<ReferralResponseDto>>> GetAllAsync(
        int page,
        int pageSize,
        string? search,
        string? status,
        CancellationToken cancellationToken)
        {
            var result =
                await _repo.GetAllAsync(
                    page,
                    pageSize,
                    search,
                    status);

            return new ApiResponse<
                PagedResult<ReferralResponseDto>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Referrals retrieved successfully",

                Data =
                    new PagedResult<ReferralResponseDto>
                    {
                        Items =
                            result.Items
                                .Select(MapToResponse)
                                .ToList(),

                        Page = result.Page,
                        PageSize = result.PageSize,
                        TotalCount = result.TotalCount,
                        TotalPages = result.TotalPages
                    }
            };
        }

        public async Task<ApiResponse<ReferralResponseDto>>
    GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
        {
            try
            {
                var entity =
                    await _repo.GetByIdAsync(
                        id,
                        cancellationToken);

                if (entity == null)
                {
                    return new ApiResponse<
                        ReferralResponseDto>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message =
                            "Referral not found"
                    };
                }

                return new ApiResponse<
                    ReferralResponseDto>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Referral retrieved successfully",

                    Data =
                        MapToResponse(entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    ReferralResponseDto>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve referral",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<ReferralResponseDto>> CreateAsync(
         CreateReferralDto dto,
         CancellationToken cancellationToken = default)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await ValidateRelationsExist(
                    dto.PatientId,
                    dto.ProviderId,
                    dto.CareManagerId,
                    cancellationToken);

                if (dto.CreatedDate.HasValue &&
                    dto.CreatedDate.Value > DateTime.UtcNow)
                {
                    return new ApiResponse<
                        ReferralResponseDto>
                    {
                        Success = false,
                        StatusCode = 400,
                        Message =
                            "CreatedDate cannot be in the future."
                    };
                }

                var entity = new Referral
                {
                    PatientId = dto.PatientId,
                    ProviderId = dto.ProviderId,
                    CareManagerId = dto.CareManagerId,
                    CreatedDate =
                        dto.CreatedDate ??
                        DateTime.UtcNow,
                    Status = dto.Status,
                    Priority = dto.Priority
                };

                var created =
                    await _repo.CreateAsync(
                        entity,
                        cancellationToken);

                var referral =
                    await _repo.GetByIdAsync(
                        created.ReferralId,
                        cancellationToken);
                stopwatch.Stop();

                _logger.LogInformation(
                    "Performance | Service: ReferralService | Method: CreateAsync | Execution Time: {ElapsedMilliseconds} ms",
                    stopwatch.ElapsedMilliseconds);

                if (stopwatch.ElapsedMilliseconds > 1000)
                {
                    _logger.LogWarning(
                        "Performance Warning | CreateAsync took {ElapsedMilliseconds} ms",
                        stopwatch.ElapsedMilliseconds);
                }
                _cache.Remove(CacheKeys.HospitalDashboard);

                await _notificationService.RefreshDashboard();

                await _notificationService.RefreshReferrals();
                return new ApiResponse<
                    ReferralResponseDto>
                {
                    Success = true,
                    StatusCode = 201,
                    Message =
                        "Referral created successfully",

                    Data =
                        MapToResponse(referral!)
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogInformation(
                    "Performance | Service: ReferralService | Method: CreateAsync | Failed after {ElapsedMilliseconds} ms",
                    stopwatch.ElapsedMilliseconds);

                return new ApiResponse<ReferralResponseDto>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to create referral",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }
        public async Task<ApiResponse<object>>UpdateStatusAsync(
         int referralId,
         AuthorizationStatus status,
         CancellationToken cancellationToken)
        {
            try
            {
                var referral =
                    await _repo.GetByIdAsync(
                        referralId,
                        cancellationToken);

                if (referral == null)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message =
                            "Referral not found"
                    };
                }

                referral.Status = status;

                await _repo.UpdateAsync(
                    referral,
                    cancellationToken);
                _cache.Remove(CacheKeys.HospitalDashboard);

                await _notificationService.RefreshDashboard();

                await _notificationService.RefreshReferrals();
                return new ApiResponse<object>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Referral status updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to update status",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<object>>UpdateAsync(
        int id,
        UpdateReferralDto dto,
        CancellationToken cancellationToken = default)
        {
            try
            {
                var existing =
                    await _repo.GetByIdAsync(
                        id,
                        cancellationToken);

                if (existing == null)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message =
                            "Referral not found"
                    };
                }

                await ValidateRelationsExist(
                    dto.PatientId,
                    dto.ProviderId,
                    dto.CareManagerId,
                    cancellationToken);

                if (dto.CreatedDate.HasValue &&
                    dto.CreatedDate.Value >
                    DateTime.UtcNow)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        StatusCode = 400,
                        Message =
                            "CreatedDate cannot be in the future."
                    };
                }

                existing.PatientId =
                    dto.PatientId;

                existing.ProviderId =
                    dto.ProviderId;

                existing.CareManagerId =
                    dto.CareManagerId;

                existing.CreatedDate =
                    dto.CreatedDate ??
                    existing.CreatedDate;

                existing.Status =
                    dto.Status;

                existing.Priority =
                    dto.Priority;

                await _repo.UpdateAsync(
                    existing,
                    cancellationToken);
                _cache.Remove(CacheKeys.HospitalDashboard);

                await _notificationService.RefreshDashboard();

                await _notificationService.RefreshReferrals();

                return new ApiResponse<object>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Referral updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to update referral",

                    Errors = new()
                        {
                            ex.Message
                        }
                };
            }
        }

        public async Task<ApiResponse<object>>DeleteAsync(
         int id,
         CancellationToken cancellationToken = default)
        {
            try
            {
                var entity =
                    await _repo.GetByIdAsync(
                        id,
                        cancellationToken);

                if (entity == null)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message =
                            "Referral not found"
                    };
                }

                await _repo.DeleteAsync(
                    id,
                    cancellationToken);
                _cache.Remove(CacheKeys.HospitalDashboard);

                await _notificationService.RefreshDashboard();

                await _notificationService.RefreshReferrals();
                return new ApiResponse<object>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Referral deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to delete referral",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<List<ReferralResponseDto>>>GetByPatientIdAsync(
        int patientId,
        CancellationToken cancellationToken = default)
        {
            try
            {
                var items =
                    await _repo.GetByPatientIdAsync(
                        patientId,
                        cancellationToken);

                return new ApiResponse<
                    List<ReferralResponseDto>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Patient referrals retrieved successfully",

                    Data =
                        items.Select(MapToResponse)
                             .ToList()
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    List<ReferralResponseDto>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve referrals",

                    Errors = new()
            {
                ex.Message
            }
                };
            }
        }

        public async Task<ApiResponse<HospitalPagedResponse<ReferralTrackingResponseDto>>> GetByCareManagerIdAsync(
      int careManagerId,
      int page,
      int pageSize,
      string? search = null,
      AuthorizationStatus? status = null,
      CancellationToken cancellationToken = default)
        {
            try
            {
                var referrals = await _repo.GetByCareManagerIdAsync(
                    careManagerId,
                    page,
                    pageSize,
                    search,
                    status,
                    cancellationToken);

                return new ApiResponse<HospitalPagedResponse<ReferralTrackingResponseDto>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Referrals retrieved successfully.",
                    Data = referrals
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<HospitalPagedResponse<ReferralTrackingResponseDto>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve referrals.",
                    Errors = new()
            {
                ex.Message
            }
                };
            }
        }
        public async Task<ApiResponse<List<ReferralResponseDto>>>GetByProviderIdAsync(
        int userId, ProviderReferralQueryDto query,
        CancellationToken cancellationToken = default)
        {
            try
            {
                var items =
                    await _repo.GetByProviderIdAsync(
                        userId,query,
                        cancellationToken);

                return new ApiResponse<
                    List<ReferralResponseDto>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Provider referrals retrieved successfully",

                    Data =
                        items.Select(MapToResponse)
                             .ToList()
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    List<ReferralResponseDto>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve referrals",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        
    
        public async Task<ApiResponse<List<ReferralResponseDto>>>GetPendingReferralsAsync(
        CancellationToken cancellationToken = default)
        {
            try
            {
                var items =
                    await _repo.GetPendingReferralsAsync(
                        cancellationToken);

                return new ApiResponse<
                    List<ReferralResponseDto>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Pending referrals retrieved successfully",

                    Data =
                        items.Select(MapToResponse)
                             .ToList()
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    List<ReferralResponseDto>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve referrals",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<List<ReferralResponseDto>>>GetCompletedReferralsAsync(
        CancellationToken cancellationToken = default)
        {
            try
            {
                var items =
                    await _repo.GetCompletedReferralsAsync(
                        cancellationToken);

                return new ApiResponse<
                    List<ReferralResponseDto>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Completed referrals retrieved successfully",

                    Data =
                        items.Select(MapToResponse)
                             .ToList()
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    List<ReferralResponseDto>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve referrals",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        private ReferralResponseDto MapToResponse(Referral r)
        {
            return new ReferralResponseDto
            {
                ReferralId = r.ReferralId,
                PatientId = r.PatientId,
                ProviderId = r.ProviderId,
                CareManagerId = r.CareManagerId,
                CreatedDate = r.CreatedDate,
                Status = r.Status.ToString(),
                Priority = r.Priority.ToString(),
                PatientName = r.patient is null ? null : $"{r.patient.FirstName} {r.patient.LastName}",
                ProviderName = r.provider?.ProviderName,
                CareManagerName = r.careManager is null ? null : $"{r.careManager.FirstName} {r.careManager.LastName}"
            };
        }
        public async Task<ApiResponse<List<ReferralResponseDto>>>GetPendingByProviderIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
        {
            try
            {
                var items =
                    await _repo.GetPendingByProviderIdAsync(
                        userId,
                        cancellationToken);

                return new ApiResponse<
                    List<ReferralResponseDto>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Pending Provider referrals retrieved successfully",

                    Data =
                        items.Select(MapToResponse)
                             .ToList()
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    List<ReferralResponseDto>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve pending referrals",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<ReferralResponseDto>> AcceptReferralAsync(int referralId)
        {
            var referral = await _repo.GetByIdAsync(referralId);

            if (referral == null)
            {
                return new ApiResponse<ReferralResponseDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Referral not found"
                };
            }

            referral.Status = AuthorizationStatus.Approved;

            
                await _repo.UpdateAsync(referral);

            _cache.Remove(CacheKeys.HospitalDashboard);

            await _notificationService.RefreshDashboard();

            await _notificationService.RefreshReferrals();

            return new ApiResponse<ReferralResponseDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Referral accepted successfully",
                
            };
        }

        public async Task<ApiResponse<ReferralDetailsDto>> GetReferralDetailsAsync(int UserId,int referralId)
        {
            var referral =
                await _repo.GetReferralDetailsAsync(
                    UserId,
                    referralId);

            if (referral == null)
            {
                return new ApiResponse<ReferralDetailsDto>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "Referral not found"
                };
            }

            return new ApiResponse<ReferralDetailsDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Referral details retrieved successfully",
                Data = referral
            };
        }

        public async Task<ApiResponse<ProviderDashboardDto>> GetDashboardSummaryAsync(int userId)
        {
            var dashboard =
                await _repo.GetDashboardSummaryAsync(userId);

            return new ApiResponse<ProviderDashboardDto>
            {
                Success = true,
                StatusCode = 200,
                Message = "Dashboard summary retrieved successfully",
                Data = dashboard
            };
        }

        private async Task ValidateRelationsExist(int patientId, int providerId, int careManagerId, CancellationToken cancellationToken)
        {
            var patientExists = await _db.Patients.AnyAsync(p => p.PatientId == patientId, cancellationToken);
            if (!patientExists) throw new KeyNotFoundException($"Patient {patientId} not found.");

            var providerExists = await _db.PostAcuteProviders.AnyAsync(p => p.ProviderId == providerId, cancellationToken);
            if (!providerExists) throw new KeyNotFoundException($"Provider {providerId} not found.");

            var careManagerExists = await _db.Users.AnyAsync(u => u.UserId == careManagerId, cancellationToken);
            if (!careManagerExists) throw new KeyNotFoundException($"Care manager {careManagerId} not found.");
        }
    }
}