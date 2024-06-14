using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Reference.Api.Dtos.Requests;

namespace Reference.Api.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> All();

        Task<T?> GetById(Guid id);

        Task<bool> Add(T entity);

        bool Delete(T entity);

        bool Upsert(T entity);

        IQueryable<T> GetEntities(Expression<Func<T, bool>> predicate);

        Task<bool> Any(Guid id);

        //Task<(IEnumerable<T> items, int totalCount)> GetPaged(PaginationParameters paginationParameters);

    }
}

