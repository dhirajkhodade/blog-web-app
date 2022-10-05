using System.Security.Claims;
using Microsoft.Extensions.Logging;
using GeekSpot.UI.Controllers;
using GeekSpot.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GeekSpot.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using GeekSpot.UI.Models;

namespace GeekSpot.Tests
{
    public class HomeControllerTests
    {
        private Mock<ILogger<HomeController>> _mockLogger;
        private Mock<IBlogRepositoy> _mockBlogRepository;

        public HomeControllerTests()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _mockBlogRepository = new Mock<IBlogRepositoy>();

            _mockBlogRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Post, bool>>>()))
                .ReturnsAsync((Expression<Func<Post,bool>> captured) => 
                {
                    return GetSamplePosts()
                        .Where(captured.Compile()); 
                });
        }


        [Fact]
        public void IsUserAuthenticated()
        {
            var controller = CreateHomeControllerAs("Dhiraj");
            Assert.Equal("Dhiraj", controller?.User?.Identity?.Name);
        }

        [Fact]
        public async Task GetPostsByTag_ReturnsAViewResult_WithMatchingPost_Positive()
        {
            var controller = CreateHomeControllerAs("Dhiraj");
            // Act
            var result = await controller.GetPostsByTag("tag1") as ViewResult;
            // Assert
            var data = Assert.IsAssignableFrom<IndexViewModel>(result?.ViewData.Model);
            Assert.Equal(1, data?.Posts.ToList().Count);
            Assert.Equal("tag1", data?.Posts.ToList()[0].Tags[0].Name);
        }

        [Fact]
        public async Task GetPostsByTag_ReturnsAViewResult_WithMatchingPost_Negative()
        {
            var controller = CreateHomeControllerAs("Dhiraj");
            // Act
            var result = await controller.GetPostsByTag("tag2") as ViewResult;
            // Assert
            var data = Assert.IsAssignableFrom<IndexViewModel>(result?.ViewData.Model);
            Assert.Equal(0,data?.Posts.ToList().Count);
        }

        HomeController CreateHomeControllerAs(string userName)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.Name, userName)
                                   }, "TestAuthentication"));
            var controller = new HomeController(_mockLogger.Object, _mockBlogRepository.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            return controller;
        }

        List<Post> GetSamplePosts()
        {
            var posts = new List<Post>();
            posts.Add(new Post() { 
                Id = 1, 
                Content = "test content",
                Title = "test title",
                Published = true,
                ReadCount = 1,
                CreatedOn = DateTime.Now,
                LastModifiedOn = DateTime.Now,
                PublishedOn = DateTime.Now,
                Author = new Author() { Id=1,Name="Dhiraj", Surname="Khodade",  Description ="Developer"},
                AuthorId = 1,
                Tags = new List<Tag>() { new Tag() { Id=1, Name="tag1" } }
            });
            return posts;
        }


        
    }
}