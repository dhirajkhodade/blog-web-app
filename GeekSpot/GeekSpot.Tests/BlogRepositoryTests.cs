using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using GeekSpot.Domain.Entities;
using GeekSpot.Core.Repositories;
using GeekSpot.Core;

namespace GeekSpot.Tests
{
    public class BlogRepositoryTests
    {
        private Mock<ILogger<BlogRepository>> _mockLogger;
        private DbContextOptions<AppDbContext> _mockDbContextOptions;

        public BlogRepositoryTests()
        {
            _mockLogger = new Mock<ILogger<BlogRepository>>();

            _mockDbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                                 .UseInMemoryDatabase(databaseName: "GeekSpotDb")
                                 .Options;
            //seed data
            using (var context = new AppDbContext(_mockDbContextOptions))
            {
                context.Authors.Add(new Author() { Id = 1, Name = "Dhiraj", Surname = "Khodade", Description = "Developer" ,
                Posts = new List<Post>() {
                new Post()
                {
                  Id = 1, Content = "test content", Title = "test title", Published = true, ReadCount = 1, CreatedOn = DateTime.Now, LastModifiedOn = DateTime.Now, PublishedOn = DateTime.Now, Author = new Author() { Id = 1, Name = "Dhiraj", Surname = "Khodade", Description = "Developer" }, AuthorId = 1, Tags = new List<Tag>() { new Tag() { Id = 1, Name = "tag1" } }
                },
                new Post()
                {
                   Id = 2, Content = "test content1", Title = "test title1", Published = true, ReadCount = 10, CreatedOn = DateTime.Now, LastModifiedOn = DateTime.Now, PublishedOn = DateTime.Now, Author = new Author() { Id = 1, Name = "Dhiraj", Surname = "Khodade", Description = "Developer" }, AuthorId = 1, Tags = new List<Tag>() { new Tag() { Id = 2, Name = "tag2" } }
                }
                }
                });
                
                context.SaveChanges();
            }
        }

        [Fact]
        public async Task TestGetAllUsers()
        {
            using (var context = new AppDbContext(_mockDbContextOptions))
            {
                var blogRepository = new BlogRepository(context, _mockLogger.Object);
                var posts = await blogRepository.GetAllAsync();

                Assert.Equal(2, posts.Count());
                Assert.True(posts?.Any(p => p.Id == 1));
                Assert.True(posts?.Any(p => p.Id == 2));

                Assert.True(posts?.Any(p => { 
                    return p.Tags.Any(t => t.Name.Equals("tag1")); 
                }));

                Assert.True(posts?.Any(p => {
                    return p.Tags.Any(t => t.Name.Equals("tag2"));
                }));
            }
        }

    }
}
