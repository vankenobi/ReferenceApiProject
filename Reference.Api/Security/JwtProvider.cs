using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Reference.Api.Models;

namespace Reference.Api.Security
{
	public class JwtProvider : IJwtProvider
	{
        private readonly JwtOptions _options;

		public JwtProvider(IOptions<JwtOptions> options)
		{
            _options = options.Value;
		}

        public string Generate(User user)
        {
            var expirationDate = DateTime.UtcNow.AddHours(1);

            var claims = new Claim[]
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email)
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                claims,
                null,
                expirationDate,
                signingCredentials);

            string tokenValue = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return tokenValue;
        }
    }
}

