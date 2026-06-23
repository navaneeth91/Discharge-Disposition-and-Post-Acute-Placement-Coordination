using DischargeDisposition_Backend.Hospital.Models;

namespace DischargeDisposition_Backend.Hospital.Repositories.Interfaces
{
    public interface IDispositionTypeRepository
    {
        Task<List<DispositionType>> GetAllAsync();
       
    }
}