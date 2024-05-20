using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using UserService.Controllers;
using UserService.Models;
using Xunit;

namespace UserService.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<UserContext> _mockContext;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockContext = new Mock<UserContext>(new DbContextOptions<UserContext>());
            _mockConfiguration = new Mock<IConfiguration>();
            _controller = new UserController(_mockContext.Object, _mockConfiguration.Object);
        }


        [Fact]
        public async Task SignIn_ReturnsOkResult_WhenUserSignedInSuccessfully()
        {
            // Arrange
            var user = new User { Email = "test@example.com", Password = "password" };
            _mockContext.Setup(c => c.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password)).Returns(user);

            // Act
            var result =  _controller.SignIn(user) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
        }
    }
}
