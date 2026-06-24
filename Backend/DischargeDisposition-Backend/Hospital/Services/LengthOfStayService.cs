using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class LengthOfStayService
        : ILengthOfStayService
    {
        private readonly
            ILengthOfStayRepository
            _repository;

        public LengthOfStayService(
            ILengthOfStayRepository repository)
        {
            _repository = repository;
        }

        public async Task<
            ApiResponse<LOSResponseDto>>
            GetPatientLOSAsync(
                int patientId)
        {
            try
            {
                var los =
                    await _repository
                        .GetLosByPatientIdAsync(
                            patientId);

                if (los == null)
                {
                    return new ApiResponse<
                        LOSResponseDto>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message =
                            "Patient LOS record not found"
                    };
                }

                var result =
                    new LOSResponseDto
                    {
                        PatientId =
                            los.PatientId,

                        PatientName =
                            $"{los.patient.FirstName} " +
                            $"{los.patient.LastName}",

                        AdmissionDate =
                            los.patient.AdmissionDate,

                        LastCalculatedDate =
                            los.LastCalculatedDate,

                        VarianceDays =
                            los.VarianceDays
                    };

                return new ApiResponse<
                    LOSResponseDto>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "Length of stay retrieved successfully",

                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    LOSResponseDto>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve LOS data",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<
            ApiResponse<List<LOSResponseDto>>>
            GetAllLOSAsync()
        {
            try
            {
                var records =
                    await _repository
                        .GetAllLosAsync();

                if (!records.Any())
                {
                    return new ApiResponse<
                        List<LOSResponseDto>>
                    {
                        Success = false,
                        StatusCode = 404,
                        Message =
                            "No LOS records found"
                    };
                }

                var result =
                    records.Select(los =>
                        new LOSResponseDto
                        {
                            PatientId =
                                los.PatientId,

                            PatientName =
                                $"{los.patient.FirstName} " +
                                $"{los.patient.LastName}",

                            AdmissionDate =
                                los.patient.AdmissionDate,

                            LastCalculatedDate =
                                los.LastCalculatedDate,

                            VarianceDays =
                                los.VarianceDays
                        })
                    .ToList();

                return new ApiResponse<
                    List<LOSResponseDto>>
                {
                    Success = true,
                    StatusCode = 200,
                    Message =
                        "LOS records retrieved successfully",

                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<
                    List<LOSResponseDto>>
                {
                    Success = false,
                    StatusCode = 500,
                    Message =
                        "Failed to retrieve LOS records",

                    Errors = new()
                    {
                        ex.Message
                    }
                };
            }
        }
    }
}