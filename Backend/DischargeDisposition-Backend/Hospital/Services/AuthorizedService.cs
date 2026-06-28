using DischargeDisposition_Backend.Data;
using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using DischargeDisposition_Backend.Insurance.Models;
using System.Diagnostics;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class AuthorizedService
        : IAuthorizedService
    {
        private readonly IAuthorizationRepository _repository;

        private readonly InsuranceDbContext _insuranceContext;
        private readonly ILogger<AuthorizedService> _logger;
        public AuthorizedService(
            IAuthorizationRepository repository,
            InsuranceDbContext insuranceContext,
            ILogger<AuthorizedService> logger)
        {
            _repository = repository;
            _insuranceContext = insuranceContext;
            _logger = logger;
        }

        public async Task<ApiResponse<long>>CreateAsync(CreateAuthorizationRequest dto)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var externalId =
                    $"AUTH-{Guid.NewGuid():N}"
                    .Substring(0, 12);

                var tracking =
                    new AuthorizationTracking
                    {
                        PatientId = dto.PatientId,
                        ReferralId = dto.ReferralId,
                        PayerId = dto.PayerId,
                        ExternalAuthorizationId =
                            externalId,

                        Status =
                            AuthorizationStatus.Pending,

                        RequestedDate =
                            DateTime.UtcNow,

                        LastUpdated =
                            DateTime.UtcNow
                    };

                await _repository.AddAsync(tracking);

                await _repository.SaveAsync();

                var request =
                    new AuthorizationRequest
                    {
                        MemberId =
                            dto.MemberId,

                        RequestingOrganization =
                            dto.RequestingOrganization,

                        ServiceType =
                            dto.ServiceType,

                        RequestedDate =
                            DateTime.UtcNow,

                        Status =
                            AuthorizationStatus.Pending
                    };

                _insuranceContext.AuthorizationRequests.Add(request);

                await _insuranceContext
                    .SaveChangesAsync();

                tracking.InsuranceAuthorizationRequestId = request.AuthorizationRequestId;

                await _repository.SaveAsync();
                stopwatch.Stop();

                _logger.LogInformation(
                    "Performance | Service: AuthorizationService | Method: CreateAsync | Execution Time: {ElapsedMilliseconds} ms",
                    stopwatch.ElapsedMilliseconds);

                if (stopwatch.ElapsedMilliseconds > 1000)
                {
                    _logger.LogWarning(
                        "Performance Warning | CreateAsync took {ElapsedMilliseconds} ms",
                        stopwatch.ElapsedMilliseconds);
                }
                return new ApiResponse<long>
                {
                    Success = true,
                    StatusCode = 201,
                    Message = "Authorization created successfully",
                    Data = tracking.AuthorizationId
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogInformation(
                    "Performance | Service: AuthorizationService | Method: CreateAsync | Failed after {ElapsedMilliseconds} ms",
                    stopwatch.ElapsedMilliseconds);
                return new ApiResponse<long>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to create authorization",
                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<AuthorizationResponse>>GetAsync(long authorizationId)
        {
            try
            {
                var authorization =
                    await _repository.GetAsync(
                        authorizationId);

                if (authorization == null)
                {
                    return new ApiResponse<AuthorizationResponse>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message = "Authorization not found"
                    };
                }

                return new ApiResponse<AuthorizationResponse>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Authorization retrieved successfully",
                    Data = new AuthorizationResponse
                    {
                        AuthorizationId =
                            authorization.AuthorizationId,

                        ExternalAuthorizationId =
                            authorization.ExternalAuthorizationId,

                        Status =
                            authorization.Status,

                        RequestedDate =
                            authorization.RequestedDate,

                        ResponseDate =
                            authorization.ResponseDate,

                        PayerName =
                            authorization.payer.PayerName,

                        PatientName =
                            authorization.patient.FirstName
                            + " "
                            + authorization.patient.LastName,

                        DenialReason =
                            authorization.DenialReason
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<AuthorizationResponse>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve authorization",
                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<List<AuthorizationResponse>>>GetPatientAuthorizationsAsync(int patientId)
        {
            try
            {
                var authorizations = await _repository
                        .GetByPatientAsync(patientId);

                var result =
                    authorizations
                    .Select(a =>
                        new AuthorizationResponse
                        {
                            AuthorizationId =
                                a.AuthorizationId,

                            ExternalAuthorizationId =
                                a.ExternalAuthorizationId,

                            Status =
                                a.Status,

                            RequestedDate =
                                a.RequestedDate,

                            ResponseDate =
                                a.ResponseDate,

                            PayerName =
                                a.payer.PayerName,

                            PatientName =
                                a.patient.FirstName
                                + " "
                                + a.patient.LastName,

                            DenialReason =
                                a.DenialReason
                        })
                    .ToList();

                return new ApiResponse<List<AuthorizationResponse>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Authorizations retrieved successfully",

                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<AuthorizationResponse>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve authorizations",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }
        public async Task ProcessWebhookAsync(
            AuthorizationWebhook dto)
        {
            var authorization =
                await _repository
                    .GetByInsuranceRequestIdAsync(
                        dto.AuthorizationRequestId);

            if (authorization == null)
            {
                throw new KeyNotFoundException(
                    "Authorization not found.");
            }

            if (authorization.Status ==
                dto.Status)
            {
                return;
            }

            authorization.Status =
                dto.Status;

            authorization.ResponseDate =
                dto.DecisionDate;

            authorization.DenialReason =
                dto.ReasonCode;

            authorization.LastUpdated =
                DateTime.UtcNow;

            await _repository.SaveAsync();
        }
        public async Task<ApiResponse<HospitalPagedResponse<AuthorizationTrackingResponseDto>>>
    GetByCareManagerIdAsync(
    int careManagerId,
    int page,
    int pageSize,
    string? search = null,
    AuthorizationStatus? status = null,
    CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _repository
                    .GetByCareManagerIdAsync(
                        careManagerId,
                        page,
                        pageSize,
                        search,
                        status,
                        cancellationToken);

                return new ApiResponse<HospitalPagedResponse<AuthorizationTrackingResponseDto>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Authorization tracking retrieved successfully.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<HospitalPagedResponse<AuthorizationTrackingResponseDto>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve authorization tracking.",
                    Errors = new List<string>
            {
                ex.Message
            }
                };
            }
        }
    }
}