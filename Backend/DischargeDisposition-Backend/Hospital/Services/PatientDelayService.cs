using DischargeDisposition_Backend.DTOs.Requests;
using DischargeDisposition_Backend.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.DTOs.Requests;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class PatientDelayService : IPatientDelayService
    {
        private readonly IPatientDelayRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatientDelayService(
            IPatientDelayRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<PatientDelayResponse>>CreateAsync(CreatePatientDelayRequest request)
        {
            try
            {
                var delay = new PatientDelay
                {
                    PatientId = request.PatientId,
                    DelayReasonId = request.DelayReasonId,
                    ReportedBy = request.ReportedBy,
                    StartDate = DateTime.UtcNow
                };

                await _repository.AddAsync(delay);

                return new ApiResponse<PatientDelayResponse>
                {
                    Success = true,
                    StatusCode = 201,
                    Message = "Patient delay created successfully",
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
                return new ApiResponse<PatientDelayResponse>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to create patient delay",
                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }
        public async Task<ApiResponse<List<PatientDelayDetailsResponse>>>GetByPatientIdAsync(int patientId)
        {
            try
            {
                var delays =
                    await _repository
                        .GetByPatientIdAsync(patientId);

                if (!delays.Any())
                {
                    return new ApiResponse<List<PatientDelayDetailsResponse>>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message = "No patient delays found"
                    };
                }

                var result =
                    delays.Select(x =>
                        new PatientDelayDetailsResponse
                        {
                            PatientDelayId =
                                x.PatientDelayId,

                            PatientId =
                                x.PatientId,

                            DelayReasonId =
                                x.DelayReasonId,

                            DelayReason =
                                x.delayReason.ReasonName,

                            ReportedBy =
                                x.ReportedBy,

                            ReportedByName =
                                $"{x.reportedUser.FirstName} " +
                                $"{x.reportedUser.LastName}",

                            StartDate =
                                x.StartDate,

                            EndDate =
                                x.EndDate
                        })
                    .ToList();

                return new ApiResponse<List<PatientDelayDetailsResponse>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Patient delays retrieved successfully",

                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<PatientDelayDetailsResponse>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve patient delays",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }
    }
}