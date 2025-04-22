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
    public class UserGroupServiceTest
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private UserGroupService GetService(AppDbContext context)
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            return new UserGroupService(context, memoryCache);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllUserGroups()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var user1 = new User { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Email = "Doe@example.com" };
            var user2 = new User { Id = Guid.NewGuid(), FirstName = "Lazer", LastName = "Pig", Email = "Lazer@example.com" };

            var group1 = new Group { Id = Guid.NewGuid(), Name = "Group1" };
            var group2 = new Group { Id = Guid.NewGuid(), Name = "Group2" };

            context.Users.AddRange(user1, user2);
            context.Groups.AddRange(group1, group2);
            await context.SaveChangesAsync();

            context.UserGroups.AddRange(
                new UserGroup { Id = Guid.NewGuid(), UserId = user1.Id, GroupId = group1.Id },
                new UserGroup { Id = Guid.NewGuid(), UserId = user2.Id, GroupId = group2.Id }
            );
            await context.SaveChangesAsync();

            var result = await service.GetAllAsync();
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectUserGroup()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var user = new User { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            var group = new Group { Id = Guid.NewGuid(), Name = "Admin" };

            context.Users.Add(user);
            context.Groups.Add(group);
            await context.SaveChangesAsync();

            var userGroup = new UserGroup
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                GroupId = group.Id
            };

            context.UserGroups.Add(userGroup);
            await context.SaveChangesAsync();

            var result = await service.GetByIdAsync(userGroup.Id);

            Assert.NotNull(result);
            Assert.Equal(userGroup.Id, result.Id);
            Assert.Equal(user.FirstName, result.FirstName);
            Assert.Equal(user.LastName, result.LastName);
            Assert.Equal(group.Name, result.GroupName);
        }

        [Fact]
        public async Task AddAsync_AddsUserGroup()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var newUserGroup = new UserGroupDto
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                GroupId = Guid.NewGuid()
            };

            await service.CreateAsync(newUserGroup);

            var saved = await context.UserGroups.FindAsync(newUserGroup.UserId, newUserGroup.GroupId);
            Assert.NotNull(saved);
            Assert.Equal(newUserGroup.UserId, saved.UserId);
        }

        [Fact]
        public async Task DeleteAsync_RemovesUserGroup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                            .UseInMemoryDatabase(Guid.NewGuid().ToString())
                            .Options;

            Guid userId = Guid.NewGuid();
            Guid groupId = Guid.NewGuid();

            using (var context = new AppDbContext(options))
            {
                var userGroup = new UserGroup
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    GroupId = groupId
                };

                context.UserGroups.Add(userGroup);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var service = GetService(context);
                var userGroupToDelete = await context.UserGroups.FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GroupId == groupId);
                await service.DeleteAsync(userGroupToDelete!.Id);
            }

            using (var context = new AppDbContext(options))
            {
                var deleted = await context.UserGroups
                    .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GroupId == groupId);
                Assert.Null(deleted);
            }
        }
    }
}
