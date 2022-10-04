using GeekSpot.Domain.Entities;
using GeekSpot.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
            return await _dbContext.Authors
                .Where(expression)
                .FirstOrDefaultAsync();
        }
    }
}
