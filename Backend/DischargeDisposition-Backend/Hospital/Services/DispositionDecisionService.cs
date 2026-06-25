using DischargeDisposition_Backend.DTOs.Requests;
using DischargeDisposition_Backend.DTOs.Responses;
using DischargeDisposition_Backend.Enums;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;
using System.Security.Claims;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class DispositionDecisionService
        : IDispositionDecisionService
    {
        private readonly IDispositionDecisionRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DispositionDecisionService(
            IDispositionDecisionRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<DispositionDecisionResponse>> CreateAsync(
        CreateDispositionDecisionRequest request)
        {
            try
            {
                var decision =
                    new DispositionDecision
                    {
                        PatientId =
                            request.PatientId,

                        DispositionTypeId =
                            request.DispositionTypeId,

                        ClinicianId =
                            request.ClinicianId,

                        DepartmentId =
                            request.DepartmentId,

                        DecisionDate =
                            DateTime.UtcNow,

                        Status =
                            AuthorizationStatus.Pending,

                        Notes =
                            request.Notes,

                        ExpectedTransitionDate =
                            request.ExpectedTransitionDate
                    };

                await _repository.AddAsync(decision);

                return new ApiResponse<
                    DispositionDecisionResponse>
                {
                    Success = true,
                    StatusCode = 201,
                    Message =
                        "Disposition decision created successfully",

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
                        "Failed to create disposition decision",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<
    ApiResponse<DispositionDecisionDetailsResponse>>
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
        public async Task<
    ApiResponse<DispositionDecisionResponse>>
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

        public async Task<ApiResponse<List<AssignedPatientsResponse>>>
GetAssignedPatientsAsync()
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
                await _repository.GetAssignedPatientsAsync(physicianId);

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