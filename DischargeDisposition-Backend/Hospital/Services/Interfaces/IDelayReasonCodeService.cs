using DischargeDisposition_Backend.Hospital.Models;

namespace DischargeDisposition_Backend.Hospital.Services.Interfaces
{
    public interface IDelayReasonCodeService
    {
        Task<IEnumerable<DelayReasonCode>> GetAllAsync();
    }
}