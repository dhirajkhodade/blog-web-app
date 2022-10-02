using GeekSpot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekSpot.Domain.Interfaces
{
    public interface IBlogRepositoy: IGenericRepository<Post>
    {
        Task<IEnumerable<Post>> GetPopularPosts(int count);
    }
}
