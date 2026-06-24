namespace DischargeDisposition_Backend.Hospital.DTOs.Responses
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
