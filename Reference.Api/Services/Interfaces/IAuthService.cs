using System;
using Reference.Api.Dtos.Requests;

namespace Reference.Api.Services.Interfaces
{
	public interface IAuthService
	{
        Task<string> Login(LoginUserRequest loginUserRequest);
    }
}

