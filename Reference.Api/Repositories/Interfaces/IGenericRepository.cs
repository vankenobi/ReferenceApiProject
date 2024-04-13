using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Reference.Api.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> All(); // Task is a type that represents an asynchronous operation that can return a value

        Task<T?> GetById(Guid id);

        Task<bool> Add(T entity);

        bool Delete(T entity);

        bool Upsert(T entity);

        IQueryable<T> GetEntities(Expression<Func<T, bool>> predicate);

        Task<bool> Any(Guid id);
       
    }
}

