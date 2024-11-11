using emr_backend_data.IRepository;
using emr_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_data.Repository
{
    public class JwtManager : IJwtManager
    {
        private readonly IConfiguration _config;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly IAccountRepository _accountRepository;

        public JwtManager(IConfiguration config, IRefreshTokenGenerator refreshTokenGenerator, IAccountRepository accountRepository)
        {
            _config = config;
            _refreshTokenGenerator = refreshTokenGenerator;
            _accountRepository = accountRepository;
        }

        public async Task<AuthResponse> GenerateJsonWebToken(AccessUserVM user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["Secret"]));
            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"]);  // Get the expiration time from the settings

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.PhoneNumber, user.PhoneNumber),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),  // Use the configured expiration time
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();

            await _accountRepository.UpdateRefreshToken(refreshToken, user.UserId);

            var authResponse = new AuthResponse()
            {
                JwtToken = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken
            };

            return authResponse;
        }

        public async Task<AuthResponse> RefreshJsonWebToken(long accountId, Claim[] claims)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings["Secret"]));
            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"]);  // Get the expiration time from the settings

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),  // Use the configured expiration time
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();

            await _accountRepository.UpdateRefreshToken(refreshToken, accountId);

            var authResponse = new AuthResponse()
            {
                JwtToken = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken
            };

            return authResponse;
        }
    }
}
