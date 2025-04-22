using Application.DTOs;
using Application.Interfaces;
using BusinessLayer.Models;
using DataLayer.Data;
using DataLayer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Services
{
    public class GroupPermissionServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
                .Options;
            return new AppDbContext(options);
        }
        private GroupPermissionService GetService(AppDbContext context)
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            return new GroupPermissionService(context, memoryCache);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllGroupPermissions()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var group1 = new Group { Id = Guid.NewGuid(), Name = "Group1" };
            var group2 = new Group { Id = Guid.NewGuid(), Name = "Group2" };
            var permission1 = new Permission { Id = Guid.NewGuid(), Name = "Permission1" };
            var permission2 = new Permission { Id = Guid.NewGuid(), Name = "Permission2" };

            context.Groups.AddRange(group1, group2);
            context.Permissions.AddRange(permission1, permission2);
            await context.SaveChangesAsync();

            context.GroupPermissions.AddRange(
                new GroupPermission { Id = Guid.NewGuid(), GroupId = group1.Id, PermissionId = permission1.Id },
                new GroupPermission { Id = Guid.NewGuid(), GroupId = group2.Id, PermissionId = permission2.Id }
            );
            await context.SaveChangesAsync();

            var result = await service.GetAllAsync();
            Assert.Equal(2, result.Count());
        }


        [Fact]
        public async Task AddAsync_AddsGroupPermission()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var group = new Group { Id = Guid.NewGuid(), Name = "TestGroup" };
            var permission = new Permission { Id = Guid.NewGuid(), Name = "TestPermission" };

            context.Groups.Add(group);
            context.Permissions.Add(permission);
            await context.SaveChangesAsync();

            var newGp = new GroupPermissionDto
            {
                Id = Guid.NewGuid(),
                GroupId = group.Id,
                PermissionId = permission.Id
            };

            await service.CreateAsync(newGp);

            var saved = await context.GroupPermissions.FindAsync(newGp.GroupId, newGp.PermissionId);
            Assert.NotNull(saved);
            Assert.Equal(newGp.GroupId, saved.GroupId);
        }

        [Fact]
        public async Task DeleteAsync_RemovesGroupPermission()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var gp = new GroupPermission
            {
                Id = Guid.NewGuid(),
                GroupId = Guid.NewGuid(),
                PermissionId = Guid.NewGuid()
            };

            context.GroupPermissions.Add(gp);
            await context.SaveChangesAsync();

            await service.DeleteAsync(gp.GroupId, gp.PermissionId);

            var deleted = await context.GroupPermissions.FindAsync(gp.GroupId, gp.PermissionId);
            Assert.Null(deleted);
        }
    }
}
