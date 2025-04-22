using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDto>> GetAllAsync();
        Task<PermissionDto> GetByIdAsync(Guid id);
        Task CreateAsync(PermissionDto permissionDto);
        Task UpdateAsync(Guid id, PermissionDto permissionDto);
        Task DeleteAsync(Guid id);
    }
}
