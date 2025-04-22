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
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        private const string UserCacheKey = "users_cache";

        public UserService(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return await _cache.GetOrCreateAsync(UserCacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return await _context.Users
                    .Select(u => new UserDto { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName, Email = u.Email })
                    .ToListAsync();
            });
        }

        public async Task<UserDto?> GetByIdAsync(Guid id) =>
            await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDto { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName, Email = u.Email })
                .FirstOrDefaultAsync();

        public async Task CreateAsync(UserDto dto)
        {
            var user = new User { Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id, FirstName = dto.FirstName, LastName = dto.LastName, Email = dto.Email };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            _cache.Remove(UserCacheKey);
        }

        public async Task UpdateAsync(UserDto dto)
        {
            var user = await _context.Users.FindAsync(dto.Id);
            if (user == null) return;

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            await _context.SaveChangesAsync();
            _cache.Remove(UserCacheKey);
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            _cache.Remove(UserCacheKey);
        }

        public async Task<int> GetUserCountAsync() =>
            await _context.Users.CountAsync();

        public async Task<Dictionary<string, int>> GetUserCountPerGroupAsync()
        {
            return await _context.Groups
                .Select(g => new
                {
                    g.Name,
                    Count = g.UserGroups.Count
                })
                .ToDictionaryAsync(x => x.Name, x => x.Count);
        }
    }
}
