using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class Permission
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [NotMapped]
        public ICollection<GroupPermission> GroupPermissions { get; set; } = new List<GroupPermission>();
    }
}
