using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(Guid id);
        Task CreateAsync(UserDto dto);
        Task UpdateAsync(UserDto dto);
        Task DeleteAsync(Guid id);
        Task<int> GetUserCountAsync();
        Task<Dictionary<string, int>> GetUserCountPerGroupAsync();
    }
}
