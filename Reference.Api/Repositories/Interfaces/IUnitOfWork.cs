using System;
using Microsoft.EntityFrameworkCore;
using Reference.Api.Models;
using Reference.Api.Repositories.Implementations;

namespace Reference.Api.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
        IUserRepository GetUserRepository();
        Task CompleteAsync(); // this method will save all the changes made to the database
    }
}

