using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupDto>> GetAllAsync();
        Task<GroupDto> GetByIdAsync(Guid id);
        Task CreateAsync(GroupDto groupDto);
        Task UpdateAsync(Guid id, GroupDto groupDto);
        Task DeleteAsync(Guid id);
    }
}
