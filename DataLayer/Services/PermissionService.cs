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
    public class PermissionService : IPermissionService
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public PermissionService(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<PermissionDto>> GetAllAsync()
        {
            return await _cache.GetOrCreateAsync("permissions_all", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return await _context.Permissions
                    .Select(p => new PermissionDto { Id = p.Id, Name = p.Name })
                    .ToListAsync();
            });
        }

        public async Task<PermissionDto> GetByIdAsync(Guid id)
        {
            string cacheKey = $"permission_{id}";
            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                var permission = await _context.Permissions.FindAsync(id);
                return permission == null ? null : new PermissionDto { Id = permission.Id, Name = permission.Name };
            });
        }

        public async Task CreateAsync(PermissionDto dto)
        {
            _context.Permissions.Add(new Permission { Id = dto.Id, Name = dto.Name });
            await _context.SaveChangesAsync();
            _cache.Remove("permissions_all");
        }

        public async Task UpdateAsync(Guid id, PermissionDto dto)
        {
            var entity = await _context.Permissions.FindAsync(id);
            if (entity == null) return;
            entity.Name = dto.Name;
            await _context.SaveChangesAsync();
            _cache.Remove("permissions_all");
            _cache.Remove($"permission_{id}");
        }

        public async Task DeleteAsync(Guid id)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null) return;
            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();
            _cache.Remove("permissions_all");
            _cache.Remove($"permission_{id}");
        }
    }
}
