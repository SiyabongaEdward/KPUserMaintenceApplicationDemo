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
    public class PermissionServiceTest
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private PermissionService GetService(AppDbContext context)
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            return new PermissionService(context, memoryCache);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllPermissions()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            context.Permissions.AddRange(
                new Permission { Id = Guid.NewGuid(), Name = "Read" },
                new Permission { Id = Guid.NewGuid(), Name = "Write" }
            );
            await context.SaveChangesAsync();

            var result = await service.GetAllAsync();
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectPermission()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var permission = new Permission { Id = Guid.NewGuid(), Name = "Execute" };
            context.Permissions.Add(permission);
            await context.SaveChangesAsync();

            var result = await service.GetByIdAsync(permission.Id);

            Assert.NotNull(result);
            Assert.Equal("Execute", result.Name);
        }

        [Fact]
        public async Task AddAsync_AddsPermission()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var newPermission = new PermissionDto { Id = Guid.NewGuid(), Name = "Delete" };
            await service.CreateAsync(newPermission);

            var saved = await context.Permissions.FindAsync(newPermission.Id);
            Assert.NotNull(saved);
            Assert.Equal("Delete", saved.Name);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesPermission()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var permission = new Permission { Id = Guid.NewGuid(), Name = "OldName" };
            context.Permissions.Add(permission);
            await context.SaveChangesAsync();

            permission.Name = "NewName";
            var permissionDto = new PermissionDto { Id = permission.Id, Name = permission.Name };
            await service.UpdateAsync(permissionDto.Id, permissionDto);

            var updated = await context.Permissions.FindAsync(permission.Id);
            Assert.Equal("NewName", updated.Name);
        }

        [Fact]
        public async Task DeleteAsync_RemovesPermission()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var permission = new Permission { Id = Guid.NewGuid(), Name = "Temporary" };
            context.Permissions.Add(permission);
            await context.SaveChangesAsync();

            await service.DeleteAsync(permission.Id);

            var result = await context.Permissions.FindAsync(permission.Id);
            Assert.Null(result);
        }
    }
}
