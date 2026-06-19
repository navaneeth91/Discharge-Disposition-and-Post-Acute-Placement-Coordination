using DischargeDisposition_Backend.Hospital.Models;

namespace DischargeDisposition_Backend.Hospital.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        public IEnumerable<Patient> GetPatients();

       
    }
}
