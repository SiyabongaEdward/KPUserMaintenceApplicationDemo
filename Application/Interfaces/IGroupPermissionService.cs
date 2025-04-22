using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGroupPermissionService
    {
        Task<IEnumerable<GroupPermissionDto>> GetAllAsync();
        Task CreateAsync(GroupPermissionDto groupPermissionDto);
        Task DeleteAsync(Guid groupId, Guid permissionId);
    }
}
