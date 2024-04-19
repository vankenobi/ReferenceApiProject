using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Reference.Api.Dtos.Requests;
using Reference.Api.Dtos.Responses;
using Reference.Api.Models;
using Reference.Api.Repositories.Interfaces;
using Reference.Api.Security;
using Reference.Api.Services.Interfaces;

namespace Reference.Api.Services.Implementations
{
	public class AuthService : IAuthService
	{
		private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IAuthService> _logger;
        private readonly IJwtProvider _jwtProvider;

		public AuthService(IUnitOfWork unitOfWork,
            ILogger<IAuthService> logger,
            IJwtProvider jwtProvider)
		{
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _jwtProvider = jwtProvider ?? throw new ArgumentNullException(nameof(jwtProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> Login(LoginUserRequest loginUserRequest)
        {
            try
            {
                var user = await _unitOfWork.GetUserRepository().GetUserByEmail(loginUserRequest.Email);

                if (user == null)
                {
                    _logger.LogError("Invalid user to login");
                    throw new Exception("Invalid user to login");
                }

                var token = _jwtProvider.Generate(user);

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while login user: {ex}",ex);
                throw new Exception("Error while login user: {ex}", ex);
            }
          
        }
    }
}


