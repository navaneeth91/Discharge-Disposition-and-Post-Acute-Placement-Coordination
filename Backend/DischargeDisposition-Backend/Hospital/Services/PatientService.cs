using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class PatientService : IPatientService
    {
        private readonly ILogger<PatientService> _logger;
        private readonly IPatientRepository _repository;

        public PatientService(
            ILogger<PatientService> logger,
            IPatientRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<ApiResponse<IEnumerable<PatientResponseDto>>>
            GetPatientsAsync()
        {
            try
            {
                var patients =
                    await _repository.GetPatientsAsync();

                var result =
                    patients.Select(p =>
                        new PatientResponseDto
                        {
                            PatientId = p.PatientId,
                            FirstName = p.FirstName,
                            LastName = p.LastName,
                            AdmissionDate = p.AdmissionDate,
                            IsActive = p.IsActive
                        });

                return new ApiResponse<
                    IEnumerable<PatientResponseDto>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Patients retrieved successfully",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error retrieving patients");

                return new ApiResponse<
                    IEnumerable<PatientResponseDto>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve patients",
                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<PatientResponseDto>>
            GetPatientByIdAsync(int patientId)
        {
            try
            {
                var patient =
                    await _repository
                        .GetByIdAsync(patientId);

                if (patient == null)
                {
                    return new ApiResponse<
                        PatientResponseDto>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message =
                            "Patient not found"
                    };
                }

                return new ApiResponse<
                    PatientResponseDto>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Patient retrieved successfully",

                    Data =
                        new PatientResponseDto
                        {
                            PatientId =
                                patient.PatientId,

                            FirstName =
                                patient.FirstName,

                            LastName =
                                patient.LastName,

                            AdmissionDate =
                                patient.AdmissionDate,

                            IsActive =
                                patient.IsActive
                        }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error retrieving patient");

                return new ApiResponse<
                    PatientResponseDto>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve patient",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<ApiResponse<object>>
            DischargePatientAsync(
                int patientId,
                DateTime actualDischargeDate)
        {
            try
            {
                var patient =
                    await _repository
                        .GetTrackedByIdAsync(
                            patientId);

                if (patient == null)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message =
                            "Patient not found"
                    };
                }

                if (actualDischargeDate <
                    patient.AdmissionDate)
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        StatusCode = 400,
                        Message =
                            "Invalid discharge date"
                    };
                }

                patient.ActualDischargeDate =
                    actualDischargeDate;

                patient.IsActive = 0;

                await _repository
                    .UpdatePatientAsync(patient);

                return new ApiResponse<object>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Patient discharged successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error discharging patient");

                return new ApiResponse<object>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to discharge patient",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }
    }
}