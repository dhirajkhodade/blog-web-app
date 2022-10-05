using GeekSpot.Domain.Entities;
using GeekSpot.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace GeekSpot.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly AppDbContext _dbContext;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(AppDbContext dbContext,ILogger<UserRepository> logger )
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<Author?> FindAsync(Expression<Func<Author, bool>> expression)
        {
            try
            {
                return await _dbContext.Authors
                                  .Where(expression)
                                  .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
