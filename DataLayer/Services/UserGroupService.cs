using Application.DTOs;
using Application.Interfaces;
using BusinessLayer.Models;
using DataLayer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Services
{
    public class UserGroupService : IUserGroupService
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public UserGroupService(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<UserGroupDto>> GetAllAsync()
        {
            return await _cache.GetOrCreateAsync("usergroups_all", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return await _context.UserGroups
                    .Include(ug => ug.User)
                    .Include(ug => ug.Group)
                    .Select(ug => new UserGroupDto
                    {
                        Id = ug.Id,
                        UserId = ug.UserId,
                        GroupId = ug.GroupId,
                        FirstName = ug.User.FirstName,
                        LastName = ug.User.LastName,
                        GroupName = ug.Group.Name
                    })
                    .ToListAsync();
            });
        }

        public async Task CreateAsync(UserGroupDto dto)
        {
            var entity = new UserGroup
            {
                Id = dto.Id,
                UserId = dto.UserId,
                GroupId = dto.GroupId
            };
            _context.UserGroups.Add(entity);
            await _context.SaveChangesAsync();
            _cache.Remove("usergroups_all");
        }

        public async Task DeleteAsync(Guid id)
        {
            var userGroup = await _context.UserGroups
                .FirstOrDefaultAsync(ug => ug.Id == id);

            if (userGroup == null) return;

            _context.UserGroups.Remove(userGroup);
            await _context.SaveChangesAsync();

            _cache.Remove("usergroups_all");
            _cache.Remove($"usergroup_{id}");
        }

        public async Task<UserGroupDto?> GetByIdAsync(Guid id)
        {
            string cacheKey = $"usergroup_{id}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return await _context.UserGroups
                    .Include(ug => ug.User)
                    .Include(ug => ug.Group)
                    .Select(ug => new UserGroupDto
                    {
                        Id = ug.Id,
                        UserId = ug.UserId,
                        GroupId = ug.GroupId,
                        FirstName = ug.User.FirstName,
                        LastName = ug.User.LastName,
                        GroupName = ug.Group.Name
                    })
                    .FirstOrDefaultAsync(ug => ug.Id == id);
            });
        }

        public async Task<bool> UpdateAsync(UserGroupDto updatedDto)
        {
            var userGroup = await _context.UserGroups
                .FirstOrDefaultAsync(ug => ug.Id == updatedDto.Id);
            if (userGroup == null)
                return false;

            userGroup.UserId = updatedDto.UserId;
            userGroup.GroupId = updatedDto.GroupId;

            _context.UserGroups.Update(userGroup);
            await _context.SaveChangesAsync();

            _cache.Remove("usergroups_all");
            _cache.Remove($"usergroup_{updatedDto.Id}");
            return true;
        }
    }
}
