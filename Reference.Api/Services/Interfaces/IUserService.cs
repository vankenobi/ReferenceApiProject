using System;
using Reference.Api.Dtos.Requests;
using Reference.Api.Dtos.Responses;
using Reference.Api.Models;

namespace Reference.Api.Services.Interfaces
{
	public interface IUserService
	{
		Task<GetUserResponse> GetUserById(Guid id);
        Task<Guid> CreateUser(CreateUserRequest createUserRequest);
        Task<bool> UpdateUser(UpdateUserRequest updateUserRequest);
        Task<bool> DeleteUser(Guid id);
        Task<List<GetUserResponse>> GetAllUsers(PaginationParameters paginationParameters);
    }
}

