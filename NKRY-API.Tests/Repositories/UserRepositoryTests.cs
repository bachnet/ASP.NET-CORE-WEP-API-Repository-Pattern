using System.Linq;
using Microsoft.EntityFrameworkCore;
using NKRY_API.DataAccess.EFCore;
using NKRY_API.Domain.Entities;
using NKRY_API.Repositories;
using Xunit;
using static NKRY_API.Utilities.Constants;

namespace NKRY_API.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private ApplicationContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationContext(options);
            context.users.AddRange(
                new User { Id = 1, FirstName = "John", Role = UserRole.admin },
                new User { Id = 2, FirstName = "Jane", Role = UserRole.user }
            );
            context.SaveChanges();
            return context;
        }

        [Fact]
        public void GetUserRole_ReturnsCorrectRole()
        {
            using var context = GetContextWithData();
            var repository = new UserRepository(context);

            var role = repository.GetUserRole(1);

            Assert.Equal(UserRole.admin, role);
        }

        [Fact]
        public void GetAll_ReturnsAllUsers()
        {
            using var context = GetContextWithData();
            var repository = new UserRepository(context);

            var users = repository.GetAll();

            Assert.Equal(2, users.Count());
        }
    }
}
