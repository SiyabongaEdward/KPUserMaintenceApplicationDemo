using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorUI.Models;

namespace RazorUI.Pages.GroupPermission
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        [BindProperty]
        public GroupPermissionDto GroupPermission { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var result = await _httpClient.GetFromJsonAsync<GroupPermissionDto>($"api/grouppermission/{id}");
            if (result == null)
            {
                return NotFound();
            }

            GroupPermission = result;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var response = await _httpClient.DeleteAsync($"api/grouppermission/{GroupPermission.GroupId}/{GroupPermission.PermissionId}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/GroupPermissions/Index");
            }

            ModelState.AddModelError(string.Empty, "Error deleting Group Permission.");
            return Page();
        }
    }
}
