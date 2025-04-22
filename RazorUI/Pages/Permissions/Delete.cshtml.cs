using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorUI.Models;
using System.Text.Json;

namespace RazorUI.Pages.Permissions
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public DeleteModel(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClient = httpClientFactory.CreateClient();
            _config = config;
        }

        [BindProperty]
        public PermissionDto Permission { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var baseUrl = _config["ApiBaseUrl"];
            var response = await _httpClient.GetAsync($"{baseUrl}api/permissions/{id}");

            if (!response.IsSuccessStatusCode)
                return RedirectToPage("./Index");

            var json = await response.Content.ReadAsStringAsync();
            Permission = JsonSerializer.Deserialize<PermissionDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var baseUrl = _config["ApiBaseUrl"];
            var response = await _httpClient.DeleteAsync($"{baseUrl}api/permissions/{Permission.Id}");

            return RedirectToPage("./Index");
        }
    }
}
