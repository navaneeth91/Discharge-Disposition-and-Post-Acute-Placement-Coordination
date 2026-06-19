
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

namespace DischargeDisposition_Backend.Helpers
{
    public sealed class JwtHelper
    {
        private readonly IConfiguration _config;
        private readonly ILogger<JwtHelper> _logger;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly byte[] _keyBytes;

        public JwtHelper(IConfiguration config, ILogger<JwtHelper> logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _issuer = _config["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer missing from configuration.");
            _audience = _config["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience missing from configuration.");
            var key = _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key missing from configuration.");

            // validated key -> bytes
            _keyBytes = Encoding.UTF8.GetBytes(key);
            if (_keyBytes.Length < 16)
            {
                _logger.LogWarning("Jwt:Key appears weak (length < 16 bytes). Consider using a stronger key.");
            }
        }

        public string GenerateToken(
            int userId,
            string userName,
            string roleName,
            string? departmentName,
            TimeSpan? lifetime = null)
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentException("userName required", nameof(userName));
            if (string.IsNullOrWhiteSpace(roleName)) roleName = "UNASSIGNED";

            var now = DateTime.UtcNow;
           

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName),
                new Claim(ClaimTypes.Role, roleName)
            };

            if (!string.IsNullOrWhiteSpace(departmentName))
            {
                claims.Add(new Claim("department", departmentName));
            }

            var creds = new SigningCredentials(new SymmetricSecurityKey(_keyBytes), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: now,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}