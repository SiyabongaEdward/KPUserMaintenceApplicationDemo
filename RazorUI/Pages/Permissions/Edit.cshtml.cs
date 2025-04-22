using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using RazorUI.Models;

namespace RazorUI.Pages.Permissions
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public EditModel(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClient = httpClientFactory.CreateClient();
            _config = config;
        }

        [BindProperty]
        public PermissionDto Permission { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var baseUrl = _config["ApiBaseUrl"];
            var response = await _httpClient.GetAsync($"{baseUrl}api/permission/{id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToPage("./Index");

            var json = await response.Content.ReadAsStringAsync();
            Permission = JsonSerializer.Deserialize<PermissionDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var baseUrl = _config["ApiBaseUrl"];
            var response = await _httpClient.PutAsync(
                $"{baseUrl}api/permission/{Permission.Id}",
                new StringContent(JsonSerializer.Serialize(Permission), Encoding.UTF8, "application/json")
            );

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to update permission.");
            return Page();
        }
    }
}
