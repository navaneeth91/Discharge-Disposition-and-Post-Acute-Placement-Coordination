using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class CareManagerService : ICareManagerService
    {
        private readonly ICareManagerRepository _repository;
        private readonly ILogger<CareManagerService> _logger;

        public CareManagerService(
            ICareManagerRepository repository,
            ILogger<CareManagerService> logger)
        {
            _repository = repository ??
                throw new ArgumentNullException(nameof(repository));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApiResponse<CareManagerDashboardResponse>> GetDashboardAsync(
            int careManagerId)
        {
            try
            {
                _logger.LogInformation(
                    "Service: Retrieving dashboard for Care Manager {CareManagerId}.",
                    careManagerId);

                var dashboard =
                    await _repository.GetDashboardAsync(careManagerId);

                return new ApiResponse<CareManagerDashboardResponse>
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Dashboard retrieved successfully.",
                    Data = dashboard
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Service Error: Failed to retrieve dashboard for Care Manager {CareManagerId}.",
                    careManagerId);

                return new ApiResponse<CareManagerDashboardResponse>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Failed to retrieve dashboard.",
                    Errors = new List<string>
                    {
                        ex.Message
                    }
                };
            }
        }
    }
}