namespace RazorUI.Models
{
    public class PermissionDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
    }
}
