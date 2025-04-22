using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorUI.Models;

namespace RazorUI.Pages.GroupPermission
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        [BindProperty]
        public GroupPermissionDto GroupPermission { get; set; }

        public List<SelectListItem> GroupList { get; set; }
        public List<SelectListItem> PermissionList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadDropdownsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownsAsync();
                return Page();
            }

            var response = await _httpClient.PostAsJsonAsync("api/grouppermission", GroupPermission);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/GroupPermissions/Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to create group permission.");
            await LoadDropdownsAsync();
            return Page();
        }

        private async Task LoadDropdownsAsync()
        {
            var groups = await _httpClient.GetFromJsonAsync<List<GroupDto>>("api/group");
            var permissions = await _httpClient.GetFromJsonAsync<List<PermissionDto>>("api/permission");

            GroupList = groups.Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name }).ToList();
            PermissionList = permissions.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }).ToList();
        }
    }
}
