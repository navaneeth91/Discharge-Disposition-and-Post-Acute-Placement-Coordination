using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.Repositories.Interfaces;
using DischargeDisposition_Backend.Hospital.Services.Interfaces;

namespace DischargeDisposition_Backend.Hospital.Services
{
    public class DelayReasonCodeService : IDelayReasonCodeService
    {
        private readonly IDelayReasonCodeRepository _repository;

        public DelayReasonCodeService(
            IDelayReasonCodeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DelayReasonCode>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}