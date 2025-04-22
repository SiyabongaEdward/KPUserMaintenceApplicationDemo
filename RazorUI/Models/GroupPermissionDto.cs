namespace RazorUI.Models
{
    public class GroupPermissionDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid GroupId { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public Guid PermissionId { get; set; }
        public string PermissionName { get; set; } = string.Empty;
    }
}
