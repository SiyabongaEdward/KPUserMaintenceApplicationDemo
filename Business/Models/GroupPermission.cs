using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class GroupPermission
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        [NotMapped]
        public Group Group { get; set; } = null!;
        public Guid PermissionId { get; set; }
        [NotMapped]
        public Permission Permission { get; set; } = null!;
    }
}
