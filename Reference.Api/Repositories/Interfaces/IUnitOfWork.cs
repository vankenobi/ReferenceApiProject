using System;
using Reference.Api.Models;

namespace Reference.Api.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
        Task CompleteAsync(); // this method will save all the changes made to the database
    }
}

