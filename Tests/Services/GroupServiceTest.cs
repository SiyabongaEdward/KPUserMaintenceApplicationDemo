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
    public class GroupServiceTest
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
                .Options;

            return new AppDbContext(options);
        }

        private GroupService GetService(AppDbContext context)
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            return new GroupService(context, memoryCache);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllGroups()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            context.Groups.AddRange(
                new Group { Id = Guid.NewGuid(), Name = "Admin" },
                new Group { Id = Guid.NewGuid(), Name = "User" }
            );
            await context.SaveChangesAsync();

            var result = await service.GetAllAsync();
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectGroup()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var group = new Group { Id = Guid.NewGuid(), Name = "Managers" };
            context.Groups.Add(group);
            await context.SaveChangesAsync();

            var result = await service.GetByIdAsync(group.Id);

            Assert.NotNull(result);
            Assert.Equal("Managers", result.Name);
        }

        [Fact]
        public async Task AddAsync_AddsGroup()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var newGroup = new GroupDto { Id = Guid.NewGuid(), Name = "Editors" };
            await service.CreateAsync(newGroup);

            var savedGroup = await context.Groups.FindAsync(newGroup.Id);
            Assert.NotNull(savedGroup);
            Assert.Equal("Editors", savedGroup.Name);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesGroup()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var group = new Group { Id = Guid.NewGuid(), Name = "Initial" };
            context.Groups.Add(group);
            await context.SaveChangesAsync();

            var updatedDto = new GroupDto { Id = group.Id, Name = "Updated" };
            await service.UpdateAsync(updatedDto.Id, updatedDto);

            var updated = await context.Groups.FindAsync(group.Id);
            Assert.Equal("Updated", updated.Name);
        }

        [Fact]
        public async Task DeleteAsync_RemovesGroup()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var group = new Group { Id = Guid.NewGuid(), Name = "ToDelete" };
            context.Groups.Add(group);
            await context.SaveChangesAsync();

            await service.DeleteAsync(group.Id);

            var result = await context.Groups.FindAsync(group.Id);
            Assert.Null(result);
        }
    }
}
