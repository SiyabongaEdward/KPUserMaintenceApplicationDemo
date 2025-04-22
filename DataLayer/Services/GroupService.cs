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
    public class GroupService : IGroupService
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public GroupService(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<GroupDto>> GetAllAsync()
        {
            return await _cache.GetOrCreateAsync("AllGroups", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return await _context.Groups
                    .Select(g => new GroupDto { Id = g.Id, Name = g.Name })
                    .ToListAsync();
            });
        }


        public async Task<GroupDto> GetByIdAsync(Guid id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) return null;
            return new GroupDto { Id = group.Id, Name = group.Name };
        }

        public async Task CreateAsync(GroupDto groupDto)
        {
            _context.Groups.Add(new Group { Id = groupDto.Id, Name = groupDto.Name });
            await _context.SaveChangesAsync();
            _cache.Remove("AllGroups");
        }

        public async Task UpdateAsync(Guid id, GroupDto groupDto)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) return;
            group.Name = groupDto.Name;
            await _context.SaveChangesAsync();
            _cache.Remove("AllGroups");
        }

        public async Task DeleteAsync(Guid id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) return;
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            _cache.Remove("AllGroups");
        }
    }
}
