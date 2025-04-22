using BusinessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Group> Groups { get; set; }
        DbSet<Permission> Permissions { get; set; }
        DbSet<UserGroup> UserGroups { get; set; }
        DbSet<GroupPermission> GroupPermissions { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
