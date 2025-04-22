using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class UserGroup
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [NotMapped]
        public User User { get; set; } = null!;
        public Guid GroupId { get; set; }
        [NotMapped]
        public Group Group { get; set; } = null!;
    }
}
