using System;
using System.Security.Authentication;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Reference.Api.Dtos.Requests;
using Reference.Api.Models;
using Reference.Api.Repositories.Interfaces;
using Reference.Api.Security;
using Reference.Api.Services.Implementations;
using Reference.Api.Services.Interfaces;

namespace Reference.Api.Test
{
	[TestFixture]
	public class AuthServiceFixture
	{
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IJwtProvider> _mockJwtProvider;
        private Mock<ILogger<IAuthService>> _mockLogger;
        private AuthService _authService;

        public AuthServiceFixture()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockJwtProvider = new Mock<IJwtProvider>();
            _mockLogger = new Mock<ILogger<IAuthService>>();
            _authService = new AuthService(_mockUnitOfWork.Object, _mockLogger.Object, _mockJwtProvider.Object);
        }

        [Test]
        public async Task Login_WhenLoginSuccess_ReturnToken()
		{
            #region MockData

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

            LoginUserRequest mockLoginUserRequest = new()
            {
                Email = "john@gmail.com",
                Password = "p@ssword"
            };

            string token = "tRKG1y7SImjZoQVr4Bs7BI0WYzsyWJpLHKgdvgFdhRptjBisHwqh2Dawhoz1X6gH";

            #endregion

            #region Mocking

            _mockJwtProvider.Setup(x => x.Generate(userMockData)).Returns(token);
            _mockUnitOfWork.Setup(x => x.GetUserRepository().GetUserByEmail(userMockData.Email)).ReturnsAsync(userMockData);

            var result = await _authService.Login(mockLoginUserRequest);

            #endregion

            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(token));
        }


        [Test]
        public async Task Login_WhenUserNotFound_ReturnToken()
        {
            #region MockData

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

            LoginUserRequest mockLoginUserRequest = new()
            {
                Email = "john@gmail.com",
                Password = "p@ssword"
            };

            string token = "tRKG1y7SImjZoQVr4Bs7BI0WYzsyWJpLHKgdvgFdhRptjBisHwqh2Dawhoz1X6gH";

            #endregion

            #region Mocking

            _mockJwtProvider.Setup(x => x.Generate(userMockData)).Returns(token);
            _mockUnitOfWork.Setup(x => x.GetUserRepository().GetUserByEmail(userMockData.Email)).ReturnsAsync((User)null);

            #endregion

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await _authService.Login(mockLoginUserRequest));

        }



    }
}

