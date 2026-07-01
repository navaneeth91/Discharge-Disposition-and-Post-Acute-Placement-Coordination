using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using DischargeDisposition_Backend.Infrastructure.Caching;
using DischargeDisposition_Backend.Infrastructure.Notifications;
using DischargeDisposition_Backend.Infrastructure.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
namespace DischargeDisposition_Backend.Hospital.Services
{
    public class PatientDelayService : IPatientDelayService
    {
        private readonly IPatientDelayRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;
        private readonly NotificationService _notificationService;
        private readonly ILogger<PatientDelayService> _logger;
        private readonly IUserRepository _userRepository;

        public PatientDelayService(
            IPatientDelayRepository repository,
            IHttpContextAccessor httpContextAccessor,
            IMemoryCache cache,
            NotificationService notificationService,
            ILogger<PatientDelayService> logger,
            IUserRepository userRepository)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _notificationService = notificationService;
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<ApiResponse<PatientDelayResponse>> CreateAsync(
    CreatePatientDelayRequest request)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var userIdClaim =
                    _httpContextAccessor.HttpContext?
                    .User
                    .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?
                    .Value;

                if (string.IsNullOrWhiteSpace(userIdClaim))
                {
                    return new ApiResponse<PatientDelayResponse>
                    {
                        Success = false,
                        StatusCode = 401,
                        Message = "User not authenticated."
                    };
                }

                var reportedBy = int.Parse(userIdClaim);

                var delay = new PatientDelay
                {
                    PatientId = request.PatientId,
                    DelayReasonId = request.DelayReasonId,
                    ReportedBy = reportedBy,
                    StartDate = DateTime.UtcNow
                };

                await _repository.AddAsync(delay);

                // Invalidate Dashboard Cache
                _cache.Remove(CacheKeys.HospitalDashboard);

                _logger.LogInformation(
                    "Hospital Dashboard cache invalidated after recording delay for Patient {PatientId}.",
                    delay.PatientId);

                // Refresh Dashboard
                await _notificationService.RefreshDashboard();

                // Refresh Delay Screens
                await _notificationService.RefreshPatientDelays();

                // Notify Care Manager
                var administrators = await _userRepository.GetAdministratorsAsync();

                foreach (var administrator in administrators)
                {
                    await _notificationService.SendToUserAsync(

                        new NotificationDto
                        {
                            Title = "Patient Delay Recorded",

                            Message =
                                $"Patient #{delay.PatientId} has been marked as delayed.",

                            Type = NotificationType.Delay,

                            Priority = NotificationPriority.High,

                            CreatedAt = DateTime.UtcNow,

                            PatientId = delay.PatientId,

                            TargetUserId = administrator.UserId
                        });

                }

                stopwatch.Stop();

                _logger.LogInformation(
                    "Performance | Service: PatientDelayService | Method: CreateAsync | Execution Time: {ElapsedMilliseconds} ms",
                    stopwatch.ElapsedMilliseconds);

                if (stopwatch.ElapsedMilliseconds > 1000)
                {
                    _logger.LogWarning(
                        "Performance Warning | PatientDelayService.CreateAsync took {ElapsedMilliseconds} ms",
                        stopwatch.ElapsedMilliseconds);
                }

                return new ApiResponse<PatientDelayResponse>
                {
                    Success = true,
                    StatusCode = 201,
                    Message = "Patient delay created successfully.",
                    Data = new PatientDelayResponse
                    {
                        PatientDelayId = delay.PatientDelayId,
                        PatientId = delay.PatientId,
                        DelayReasonId = delay.DelayReasonId,
                        ReportedBy = delay.ReportedBy,
                        StartDate = delay.StartDate,
                        EndDate = delay.EndDate
                    }
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(
                    ex,
                    "Error creating patient delay.");

                _logger.LogInformation(
                    "Performance | Service: PatientDelayService | Method: CreateAsync | Failed after {ElapsedMilliseconds} ms",
                    stopwatch.ElapsedMilliseconds);

                return new ApiResponse<PatientDelayResponse>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to create patient delay.",
                    Errors = new()
            {
                ex.Message
            }
                };
            }
        }
        public async Task<ApiResponse<List<PatientDelayDetailsResponse>>>GetByPatientIdAsync(int patientId)
        {
            var delays =
                await _repository.GetByPatientIdAsync(patientId);

            var result = delays
                .Select(x => new PatientDelayDetailsResponse
                {
                    PatientDelayId = x.PatientDelayId,
                    PatientId = x.PatientId,
                    DelayReasonId = x.DelayReasonId,
                    DelayReason = x.delayReason.ReasonName,
                    ReportedBy = x.ReportedBy,
                    ReportedByName =
                        $"{x.ReportedUser.FirstName} {x.ReportedUser.LastName}",
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                })
                .ToList();

            return new ApiResponse<List<PatientDelayDetailsResponse>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Patient delays retrieved successfully",
                Data = result
            };
        }
    }
}
