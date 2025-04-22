using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GroupPermissionDto
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public Guid PermissionId { get; set; }

        public string GroupName { get; set; } = string.Empty;
        public string PermissionName { get; set; } = string.Empty;
    }
}
