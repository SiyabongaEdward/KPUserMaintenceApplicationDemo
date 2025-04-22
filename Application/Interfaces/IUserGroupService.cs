using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserGroupService
    {
        Task<IEnumerable<UserGroupDto>> GetAllAsync();
        Task<UserGroupDto> GetByIdAsync(Guid id);
        Task CreateAsync(UserGroupDto userGroupDto);
        Task DeleteAsync(Guid id);
        Task<bool> UpdateAsync(UserGroupDto updatedDto);
    }
}
