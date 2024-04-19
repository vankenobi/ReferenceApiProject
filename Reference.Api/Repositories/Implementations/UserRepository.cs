using System;
using Microsoft.EntityFrameworkCore;
using Reference.Api.Data;
using Reference.Api.Models;
using Reference.Api.Repositories.Interfaces;

namespace Reference.Api.Repositories.Implementations
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {

        private readonly DbSet<User> dbSet;
        private readonly DataContext _context;
        private readonly ILogger _logger;

        public UserRepository()
        {
        }

        public UserRepository(DataContext context, ILogger logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            dbSet = _context.Set<User>();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            try
            {
                return await dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting entity with email {email}", email);
                throw;
            }
        }
    }
}

