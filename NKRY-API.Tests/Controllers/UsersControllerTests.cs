using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NKRY_API.Controllers;
using NKRY_API.Domain.Contracts;
using NKRY_API.Domain.Entities;
using Xunit;

namespace NKRY_API.Tests.Controllers
{
    public class UsersControllerTests
    {
        [Fact]
        public async Task Getusers_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, FirstName = "John", Role = NKRY_API.Utilities.Constants.UserRole.admin },
                new User { Id = 2, FirstName = "Jane", Role = NKRY_API.Utilities.Constants.UserRole.user }
            };

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.GetAll()).Returns(users);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.User).Returns(userRepoMock.Object);

            var controller = new UsersController(unitOfWorkMock.Object);

            // Act
            var result = await controller.Getusers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnUsers = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
            Assert.Equal(2, returnUsers.Count());
        }

        [Fact]
        public async Task GetUser_ReturnsNotFoundWhenUserMissing()
        {
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns((User?)null);

            var unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(u => u.User).Returns(userRepoMock.Object);

            var controller = new UsersController(unitOfWorkMock.Object);

            var result = await controller.GetUser(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
