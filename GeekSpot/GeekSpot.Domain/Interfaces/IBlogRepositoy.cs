using GeekSpot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GeekSpot.Domain.Interfaces
{
    public interface IBlogRepositoy
    {
        Task<Post?> GetByIdAsync(int id);
        Task<IEnumerable<Post>> GetAllAsync();
        Task CreateAsync(Post entity);
        Task UpdateAsync(Post entity);
        Task DeleteAsync(Post entity);
        Task<IEnumerable<Post>> FindAsync(Expression<Func<Post, bool>> expression);
        Task<IEnumerable<Post>> GetPopularPostsAsync(int count);
    }
}
