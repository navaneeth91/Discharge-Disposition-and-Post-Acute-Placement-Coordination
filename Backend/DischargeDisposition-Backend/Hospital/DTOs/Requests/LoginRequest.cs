using System.ComponentModel.DataAnnotations;

namespace DischargeDisposition_Backend.Hospital.DTOs.Requests
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
    }
}
