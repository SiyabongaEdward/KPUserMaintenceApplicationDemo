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
    public class GroupPermissionService : IGroupPermissionService
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public GroupPermissionService(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<IEnumerable<GroupPermissionDto>> GetAllAsync()
        {
            return await _cache.GetOrCreateAsync("grouppermissions_all", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return await _context.GroupPermissions
                    .Include(gp => gp.Group)
                    .Include(gp => gp.Permission)
                    .Select(gp => new GroupPermissionDto
                    {
                        Id = gp.Id,
                        GroupId = gp.GroupId,
                        PermissionId = gp.PermissionId,
                        GroupName = gp.Group.Name,
                        PermissionName = gp.Permission.Name
                    }).ToListAsync();
            });
        }

        public async Task CreateAsync(GroupPermissionDto dto)
        {
            var entity = new GroupPermission
            {
                Id = dto.Id,
                GroupId = dto.GroupId,
                PermissionId = dto.PermissionId
            };
            _context.GroupPermissions.Add(entity);
            await _context.SaveChangesAsync();

            _cache.Remove("grouppermissions_all");
        }

        public async Task DeleteAsync(Guid groupId, Guid permissionId)
        {
            var gp = await _context.GroupPermissions
                .FirstOrDefaultAsync(gp => gp.GroupId == groupId && gp.PermissionId == permissionId);
            if (gp == null) return;

            _context.GroupPermissions.Remove(gp);
            await _context.SaveChangesAsync();

            _cache.Remove("grouppermissions_all");
        }
    }
}
