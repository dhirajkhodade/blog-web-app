using GeekSpot.Domain.Entities;
using GeekSpot.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace GeekSpot.Core.Repositories
{
    public class BlogRepository : IBlogRepositoy
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<BlogRepository> _logger;

        public BlogRepository(AppDbContext context, ILogger<BlogRepository> logger)
        {
            _dbContext = context;
            _logger = logger;
        }
        public async Task CreateAsync(Post entity)
        {
            try
            {
                var author = await _dbContext.Authors.FindAsync(1);
                if (author != null)
                {
                    author.Posts.Add(entity);
                }
                else
                {
                    //Hard coded first time creation of the only Author this app supports
                    entity.Author = new Author()
                    {
                        Id = 1,
                        Name = "Dhiraj",
                        Surname = "Khodade",
                        Description = "Software Developer"
                    };
                    _dbContext.Add(entity);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task DeleteAsync(Post entity)
        {
            try
            {
                _dbContext.Posts.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Post>> FindAsync(Expression<Func<Post, bool>> expression)
        {
            try
            {
                return await _dbContext.Posts
                              .Include(post => post.Tags)
                              .Include(post => post.Images)
                              .Include(post => post.Author)
                              .Where(expression)
                              .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }

        public async Task<IEnumerable<Post>> GetAllAsync(bool includeNonPublished = false)
        {
            try
            {
                if (includeNonPublished)
                {
                    return await _dbContext.Posts
                    .OrderByDescending(p => p.PublishedOn)
                    .Include(post => post.Tags)
                    .Include(post => post.Images)
                    .Include(post => post.Author)
                    .ToListAsync();
                }
                return await _dbContext.Posts
                    .Where(post => post.Published)
                    .OrderByDescending(p => p.PublishedOn)
                    .Include(post => post.Tags)
                    .Include(post => post.Images)
                    .Include(post => post.Author)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Post?> GetByIdAsync(int id, bool includeNonPublished = false)
        {
            try
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
                var post = await _dbContext.Posts
                    .Where(post => post.Id == id && post.Published)
                    .Include(post => post.Tags)
                    .Include(post => post.Images)
                    .Include(post => post.Author)
                    .FirstOrDefaultAsync();

                if (post != null)
                {
                    post.ReadCount += 1;
                    await UpdateAsync(post);
                }

                return post;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(Post entity)
        {
            try
            {
                var existingPost = _dbContext.Posts
                   .Where(p => p.Id == entity.Id)
                   .Include(p => p.Tags)
                   .Include(p => p.Author)
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

                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Post>> GetPopularPostsAsync(int count)
        {
            try
            {
                return await _dbContext.Posts
               .Where(post => post.Published)
               .OrderByDescending(p => p.ReadCount)
               .Include(post => post.Tags)
               .Include(post => post.Images)
               .Include(post => post.Author)
               .Take(count)
               .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }

        public async Task<IEnumerable<Tag>> GettagsAsync()
        {
            try
            {
                return await _dbContext.tags.Distinct().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
