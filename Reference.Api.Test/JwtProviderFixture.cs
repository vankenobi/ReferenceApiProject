using System;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Reference.Api.Models;
using Reference.Api.Repositories.Interfaces;
using Reference.Api.Security;
using Reference.Api.Services.Implementations;
using Reference.Api.Services.Interfaces;

namespace Reference.Api.Test
{
	[TestFixture]
	public class JwtProviderFixture
	{

        [SetUp]
        public void Setup()
        {
        }


        [Test]
		public void Generate_WhenGenerateSuccess_ReturnsToken()
		{
            User userMockData = new()
            {
                Id = Guid.NewGuid(),
                Name = "John",
                Surname = "Doe",
                Email = "john@gmail.com",
                Password = "p@ssword",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var jwtOptions = new JwtOptions
            {
                SecretKey = "2bfba44b8ed39e74c0ed9adca608c7d1",
                Issuer = "issuer",
                Audience = "audience"
            };

            var mockOptions = Options.Create(jwtOptions);
            var jwtProvider = new JwtProvider(mockOptions);

            // Act
            var token = jwtProvider.Generate(userMockData);

            // Assert
            Assert.That(token, Is.Not.Null);
            Assert.That(token, Is.Not.Empty);

            var tokenHandler = new JwtSecurityTokenHandler();
            var decodedToken = tokenHandler.ReadJwtToken(token);

            Assert.That(decodedToken.Issuer, Is.EqualTo(jwtOptions.Issuer));

            var claims = decodedToken.Claims;
            var subClaim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            var emailClaim = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);

            Assert.That(subClaim, Is.Not.Null);
            Assert.That(emailClaim, Is.Not.Null);
            Assert.That(subClaim.Value, Is.EqualTo(userMockData.Id.ToString()));
            Assert.That(emailClaim.Value, Is.EqualTo(userMockData.Email));
        }

    }
}

