using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorUI.Models;
using System.Text.Json;

namespace RazorUI.Pages.UserGroups
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        [BindProperty]
        public UserGroupDto UserGroup { get; set; } = new();

        public DeleteModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var apiBaseUrl = _configuration["ApiBaseUrl"];
            var response = await _httpClient.GetAsync($"{apiBaseUrl}api/UserGroup/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var json = await response.Content.ReadAsStringAsync();
            var userGroup = JsonSerializer.Deserialize<UserGroupDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (userGroup == null)
            {
                return NotFound();
            }
            userGroup.FullName = $"{userGroup.FirstName?.Trim()} {userGroup.LastName?.Trim()}".Trim();
            UserGroup = userGroup;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var apiBaseUrl = _configuration["ApiBaseUrl"];
            var response = await _httpClient.DeleteAsync($"{apiBaseUrl}api/UserGroup/{UserGroup.Id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to delete user group.");
            return Page();
        }
    }
}
