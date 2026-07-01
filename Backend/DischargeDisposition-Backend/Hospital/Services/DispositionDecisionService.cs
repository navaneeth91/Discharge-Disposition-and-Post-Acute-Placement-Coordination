using DischargeDisposition_Backend.DTOs.Requests;
using DischargeDisposition_Backend.DTOs.Responses;
using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using System.Security.Claims;
using DischargeDisposition_Backend.Infrastructure.Caching;
using DischargeDisposition_Backend.Infrastructure.Notifications;
using DischargeDisposition_Backend.Infrastructure.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class DispositionDecisionService
        : IDispositionDecisionService
    {
        private readonly IDispositionDecisionRepository _repository;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly NotificationService _notificationService;

        private readonly IMemoryCache _cache;

        private readonly ILogger<DispositionDecisionService> _logger;

        private readonly IUserRepository _userRepository;

        public DispositionDecisionService(
            IDispositionDecisionRepository repository,
            IHttpContextAccessor httpContextAccessor,
            NotificationService notificationService,
            IMemoryCache cache,
            ILogger<DispositionDecisionService> logger,
            IUserRepository userRepository)
        {
            _repository = repository;

            _httpContextAccessor = httpContextAccessor;

            _notificationService = notificationService;

            _cache = cache;

            _logger = logger;

            _userRepository = userRepository;
        }

        public async Task<ApiResponse<DispositionDecisionResponse>> CreateAsync(
    CreateDispositionDecisionRequest request)
{
    var stopwatch = Stopwatch.StartNew();

    try
    {
        var existingDecision =
            await _repository
                .GetByPatientIdWithTrackingAsync(
                    request.PatientId);

        DispositionDecision decision;

        if (existingDecision == null)
        {
            decision = new DispositionDecision
            {
                PatientId = request.PatientId,

                DispositionTypeId = request.DispositionTypeId,

                ClinicianId = request.ClinicianId,

                DepartmentId = request.DepartmentId,

                DecisionDate = DateTime.UtcNow,

                Status = AuthorizationStatus.Pending,

                Notes = request.Notes,

                ExpectedTransitionDate =
                    request.ExpectedTransitionDate
            };

            await _repository.AddAsync(decision);
        }
        else
        {
            existingDecision.DispositionTypeId =
                request.DispositionTypeId;

            existingDecision.DepartmentId =
                request.DepartmentId;

            existingDecision.ExpectedTransitionDate =
                request.ExpectedTransitionDate;

            existingDecision.Notes =
                request.Notes;

            existingDecision.DecisionDate =
                DateTime.UtcNow;

            existingDecision.Status =
                AuthorizationStatus.Pending;

            await _repository.UpdateDecisionAsync(
                existingDecision);

            decision = existingDecision;
        }

        // ================= Cache =================

        _cache.Remove(CacheKeys.HospitalDashboard);

        _logger.LogInformation(
            "Hospital Dashboard cache invalidated after disposition decision for Patient {PatientId}.",
            decision.PatientId);

        // ================= SignalR Refresh =================

        await _notificationService.RefreshDashboard();

        await _notificationService.RefreshAssignments();

        // ================= Notifications =================

        var administrators =
            await _userRepository
                .GetAdministratorsAsync();

        foreach (var admin in administrators)
        {
            await _notificationService
                .SendToUserAsync(
                    new NotificationDto
                    {
                        Title = "Disposition Decision Submitted",

                        Message =
                            $"A physician has submitted a disposition decision for Patient #{decision.PatientId}.",

                        Type = NotificationType.Assignment,

                        Priority = NotificationPriority.Normal,

                        CreatedAt = DateTime.UtcNow,

                        PatientId = decision.PatientId,

                        TargetUserId = admin.UserId
                    });
        }

        // ================= Performance =================

        stopwatch.Stop();

        _logger.LogInformation(
            "Performance | Service: DispositionDecisionService | Method: CreateAsync | Execution Time: {ElapsedMilliseconds} ms",
            stopwatch.ElapsedMilliseconds);

        if (stopwatch.ElapsedMilliseconds > 1000)
        {
            _logger.LogWarning(
                "Performance Warning | DispositionDecisionService.CreateAsync took {ElapsedMilliseconds} ms",
                stopwatch.ElapsedMilliseconds);
        }

        return new ApiResponse<DispositionDecisionResponse>
        {
            Success = true,
            StatusCode = 201,
            Message = existingDecision == null
                ? "Disposition decision created successfully."
                : "Disposition decision updated successfully.",

            Data = new DispositionDecisionResponse
            {
                DecisionId = decision.DecisionId,

                PatientId = decision.PatientId,

                DispositionTypeId = decision.DispositionTypeId,

                ClinicianId = decision.ClinicianId,

                DepartmentId = decision.DepartmentId,

                DecisionDate = decision.DecisionDate,

                Status = decision.Status.ToString(),

                Notes = decision.Notes,

                ExpectedTransitionDate =
                    decision.ExpectedTransitionDate
            }
        };
    }
    catch (Exception ex)
    {
        stopwatch.Stop();

        _logger.LogError(
            ex,
            "Error creating/updating disposition decision.");

        _logger.LogInformation(
            "Performance | Service: DispositionDecisionService | Method: CreateAsync | Failed after {ElapsedMilliseconds} ms",
            stopwatch.ElapsedMilliseconds);

        return new ApiResponse<DispositionDecisionResponse>
        {
            Success = false,
            StatusCode = 500,
            Message = "Failed to save disposition decision.",

            Errors = new()
            {
                ex.Message
            }
        };
    }
}

        public async Task<ApiResponse<DispositionDecisionDetailsResponse>>
    GetByPatientIdAsync(int patientId)
        {
            try
            {
                var decision =
                    await _repository
                        .GetByPatientIdAsync(patientId);

                if (decision == null)
                {
                    return new ApiResponse<
                        DispositionDecisionDetailsResponse>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message =
                            "Disposition decision not found"
                    };
                }

                return new ApiResponse<
                    DispositionDecisionDetailsResponse>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Disposition decision retrieved successfully",

                    Data =
                        new DispositionDecisionDetailsResponse
                        {
                            DecisionId =
                                decision.DecisionId,

                            PatientId =
                                decision.PatientId,

                            PatientName =
                                $"{decision.patient.FirstName} " +
                                $"{decision.patient.LastName}",

                            DispositionTypeId =
                                decision.DispositionTypeId,

                            DispositionTypeName =
                                decision.dispositionType
                                    .DispositionName,

                            ClinicianId =
                                decision.ClinicianId,

                            ClinicianName =
                                $"{decision.clinician.FirstName} " +
                                $"{decision.clinician.LastName}",

                            DepartmentId =
                                decision.DepartmentId,

                            DepartmentName =
                                decision.department.Name,

                            DecisionDate =
                                decision.DecisionDate,

                            Status =
                                decision.Status.ToString(),

                            Notes =
                                decision.Notes,

                            ExpectedTransitionDate =
                                decision.ExpectedTransitionDate
                        }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    DispositionDecisionDetailsResponse>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve disposition decision",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }
        public async Task<ApiResponse<DispositionDecisionResponse>>
    UpdateDecisionAsync(
        int decisionId,
        UpdateDispositionDecisionRequest request)
        {
            try
            {
                var decision =
                    await _repository
                        .GetByDecisionIdAsync(
                            decisionId);

                if (decision == null)
                {
                    return new ApiResponse<
                        DispositionDecisionResponse>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message =
                            "Disposition decision not found"
                    };
                }

                decision.DispositionTypeId =
                    request.DispositionTypeId;

                decision.DepartmentId =
                    request.DepartmentId;

                decision.Status =
                    request.Status;

                decision.ExpectedTransitionDate =
                    request.ExpectedTransitionDate;

                decision.Notes =
                    request.Notes;

                await _repository
                    .UpdateDecisionAsync(decision);

                return new ApiResponse<
                    DispositionDecisionResponse>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Disposition decision updated successfully",

                    Data =
                        new DispositionDecisionResponse
                        {
                            DecisionId =
                                decision.DecisionId,

                            PatientId =
                                decision.PatientId,

                            DispositionTypeId =
                                decision.DispositionTypeId,

                            ClinicianId =
                                decision.ClinicianId,

                            DepartmentId =
                                decision.DepartmentId,

                            DecisionDate =
                                decision.DecisionDate,

                            Status =
                                decision.Status.ToString(),

                            Notes =
                                decision.Notes,

                            ExpectedTransitionDate =
                                decision.ExpectedTransitionDate
                        }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    DispositionDecisionResponse>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to update disposition decision",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<List<AssignedPatientsResponse>>>GetAssignedPatientsAsync(string? search)
        {
            var userIdClaim = _httpContextAccessor
                .HttpContext?
                .User
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return new ApiResponse<List<AssignedPatientsResponse>>
                {
                    Success = false,
                    StatusCode = 401,
                    Message = "Unauthorized"
                };
            }

            int physicianId = int.Parse(userIdClaim);

            var patients =
                await _repository.GetAssignedPatientsAsync(physicianId,search);

            return new ApiResponse<List<AssignedPatientsResponse>>
            {
                Success = true,
                StatusCode = 200,
                Message = "Assigned patients retrieved successfully",
                Data = patients
            };
        }
    }
}