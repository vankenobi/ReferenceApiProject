using System;
using Reference.Api.Dtos.Responses;
using Reference.Api.Models;

namespace Reference.Api.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetUserByEmail(string email);
    }
}

