namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class PatientDistribution
        {
            public string DepartmentName { get; set; } = string.Empty;

            public int TotalPatients { get; set; }
        }
}
