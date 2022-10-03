using GeekSpot.Domain.Entities;
using GeekSpot.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GeekSpot.Core.Repositories
{
    public class BlogRepository :IBlogRepositoy
    {
        public readonly AppDbContext _dbContext;

        public BlogRepository(AppDbContext context)
        {
            _dbContext = context;
        }
        public async Task CreateAsync(Post entity)
        {
            _dbContext.Posts.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Post entity)
        {
            _dbContext.Posts.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Post>> FindAsync(Expression<Func<Post, bool>> expression)
        {
            return await _dbContext.Posts
                .Include(post => post.Tags)
                .Include(post => post.Images)
                .Include(post => post.Author)
                .Where(expression)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllAsync(bool includeNonPublished=false)
        {
            if (includeNonPublished)
            {
                return await _dbContext.Posts
                .Include(post => post.Tags)
                .Include(post => post.Images)
                .Include(post => post.Author)
                .ToListAsync();
            }
            return await _dbContext.Posts
                .Where(post => post.Published)
                .Include(post=>post.Tags)
                .Include(post=>post.Images)
                .Include(post=>post.Author)
                .ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(int id, bool includeNonPublished=false)
        {
            if (includeNonPublished)
            {
                return await _dbContext.Posts
               .Where(post => post.Id == id)
               .Include(post => post.Tags)
               .Include(post => post.Images)
               .Include(post => post.Author)
               .FirstOrDefaultAsync();
            }
            return await _dbContext.Posts
                .Where(post => post.Id == id && post.Published)
                .Include(post => post.Tags)
                .Include(post => post.Images)
                .Include(post => post.Author)
                .FirstOrDefaultAsync();

        }

        public async Task UpdateAsync(Post entity)
        {
            var existingPost = _dbContext.Posts
                    .Where(p => p.Id == entity.Id)
                    .Include(p => p.Tags)
                    .SingleOrDefault();

            if (existingPost != null)
            {
                _dbContext.Entry(existingPost).CurrentValues.SetValues(entity);

                // Delete tags
                foreach (var existingTags in existingPost.Tags.ToList())
                {
                    if (!entity.Tags.Any(t => t.Name.ToLower() == existingTags.Name.ToLower()))
                        _dbContext.tags.Remove(existingTags);
                }

                // Update and Insert tags
                foreach (var newTag in entity.Tags)
                {
                    var existingTag = existingPost.Tags
                        .Where(t => t.Name.ToLower() == newTag.Name.ToLower())
                        .SingleOrDefault();

                    if (existingTag != null)
                        // Update tag
                        _dbContext.Entry(existingTag).CurrentValues.SetValues(newTag);
                    else
                    {
                        // Insert new tag
                        var newTagToInsert = new Tag()
                        {
                            Name = newTag.Name
                        };
                        existingPost.Tags.Add(newTagToInsert);
                    }
                }

                //_dbContext.Posts.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Post>> GetPopularPostsAsync(int count)
        {
            return await _dbContext.Posts
                .Where(post=>post.Published)
                .OrderByDescending(p => p.ReadCount)
                .Include(post => post.Tags)
                .Include(post => post.Images)
                .Include(post => post.Author)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tag>> GettagsAsync()
        {
            return await _dbContext.tags.Distinct().ToListAsync();
        }

       
    }
}
