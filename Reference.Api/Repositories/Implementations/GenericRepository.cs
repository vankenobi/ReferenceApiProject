﻿using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Reference.Api.Data;
using Reference.Api.Dtos.Requests;
using Reference.Api.Models;
using Reference.Api.Repositories.Interfaces;

namespace Reference.Api.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DataContext _context;
        private readonly DbSet<T> dbSet;
        private readonly ILogger _logger;

        // constructor will take the context and logger factory as parameters
        public GenericRepository(
            DataContext context,
            ILogger logger
        )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            dbSet = _context.Set<T>();
        }

        public GenericRepository() { }

        public virtual IQueryable<T> All()
        {
            return dbSet.AsQueryable();
        }

        public virtual async Task<T?> GetById(Guid id)
        {
            try
            {
                return await dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting entity with id {Id}", id);
                throw;
            }
        }

        //public virtual async Task<(IEnumerable<T> items, int totalCount)> GetPaged(PaginationParameters paginationParameters)
        //{
        //    var totalCount = await dbSet.CountAsync();
        //    var items = await dbSet.Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
        //                            .Take(paginationParameters.PageSize)
        //                            .ToListAsync();
        //    return (items, totalCount);
        //}

        public virtual async Task<bool> Add(T entity)
        {
            try
            {
                await dbSet.AddAsync(entity);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error adding entity");
                throw;
            }
        }

        public virtual bool Delete(T entity)
        {
            try
            {
                dbSet.Remove(entity);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting entity with id {Id}", entity.Id);
                throw;
            }
        }

        public virtual async Task<bool> Any(Guid id)
        {
            return await dbSet.AnyAsync(x => x.Id == id);
        }


        public IQueryable<T> GetEntities(Expression<Func<T, bool>> predicate)
        {
            try
            {
                IQueryable<T> query = _context.Set<T>().Where(predicate);
                return query;
            }
            catch (Exception ex)
            { 
                Console.WriteLine("Error while getting entities: " + ex.Message);
                throw;
            }
        }


        public bool Upsert(T entity)
        {
            try
            {
                dbSet.Update(entity);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while updating entity: " + ex.Message);
                throw;
            }
        }
    }
}

