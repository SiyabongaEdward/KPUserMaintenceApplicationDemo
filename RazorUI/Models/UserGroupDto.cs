using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorUI.Models
{
    public class UserGroupDto
    {
        public Guid Id { get; set; } 
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;
        public Guid GroupId { get; set; }
        public string GroupName { get; set; } = string.Empty;
        [NotMapped]
        public List<SelectListItem>? Users { get; set; } = new();
        [NotMapped]
        public List<SelectListItem>? Groups { get; set; } = new();
    }
}
