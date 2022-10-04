using GeekSpot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GeekSpot.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<Author?> FindAsync(Expression<Func<Author, bool>> expression);
    }
}
