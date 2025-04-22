namespace RazorUI.Models
{
    public class GroupDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;

        public ICollection<UserGroupDto> UserGroups { get; set; } = new List<UserGroupDto>();
        public ICollection<GroupPermissionDto> GroupPermissions { get; set; } = new List<GroupPermissionDto>();
    }
}
