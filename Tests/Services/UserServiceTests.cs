using Application.DTOs;
using BusinessLayer.Models;
using DataLayer.Data;
using DataLayer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Services
{
    public class UserServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);

            context.Users.Add(new User { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Email = "john@example.com" });
            context.Users.Add(new User { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith", Email = "jane@example.com" });

            context.SaveChangesAsync();

            return context;
        }

        private UserService GetService(AppDbContext context)
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            return new UserService(context, memoryCache);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllUsers()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var result = await service.GetAllAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectUser()
        {
            var context = GetInMemoryDbContext();
            var testUser = await context.Users.FirstAsync();
            var service = GetService(context);

            var result = await service.GetByIdAsync(testUser.Id);

            Assert.NotNull(result);
            Assert.Equal(testUser.Email, result.Email);
        }

        [Fact]
        public async Task CreateAsync_AddsUser()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);
            var newUser = new UserDto
            {
                Id = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Wonderland",
                Email = "alice@example.com"
            };

            await service.CreateAsync(newUser);

            var addedUser = await context.Users.FindAsync(newUser.Id);
            Assert.NotNull(addedUser);
            Assert.Equal("Alice", addedUser.FirstName);
        }

        [Fact]
        public async Task UpdateAsync_ChangesUser()
        {
            var context = GetInMemoryDbContext();
            var user = await context.Users.FirstAsync();
            var service = GetService(context);

            var userDto = new UserDto
            {
                Id = user.Id,
                FirstName = "Updated",
                LastName = user.LastName,
                Email = user.Email
            };
            await service.UpdateAsync(userDto);

            var updatedUser = await context.Users.FindAsync(user.Id);
            Assert.Equal("Updated", updatedUser.FirstName);
        }

        [Fact]
        public async Task DeleteAsync_RemovesUser()
        {
            var context = GetInMemoryDbContext();
            var user = await context.Users.FirstAsync();
            var service = GetService(context);

            await service.DeleteAsync(user.Id);

            var deleted = await context.Users.FindAsync(user.Id);
            Assert.Null(deleted);
        }
    }
}
