using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class JwtService
    {
        private readonly string _jwtKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtService(string jwtKey, string issuer, string audience)
        {
            _jwtKey = jwtKey;
            _issuer = issuer;
            _audience = audience;
        }

        public string GenerateJwtToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[] { new Claim(JwtRegisteredClaimNames.Sub, "subject_identifier"), }
                ),
                Issuer = _issuer,
                Audience = _audience,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
