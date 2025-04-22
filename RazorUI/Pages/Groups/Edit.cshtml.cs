using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorUI.Models;

namespace RazorUI.Pages.Groups
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public EditModel(IHttpClientFactory factory, IConfiguration config)
        {
            _httpClient = factory.CreateClient();
            _config = config;
        }

        [BindProperty]
        public GroupDto Group { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_config["ApiBaseUrl"]}api/Group/{id}");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            Group = await response.Content.ReadFromJsonAsync<GroupDto>() ?? new();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var response = await _httpClient.PutAsJsonAsync($"{_config["ApiBaseUrl"]}api/Group/{Group.Id}", Group);
            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError(string.Empty, "Failed to update group.");
            return Page();
        }
    }
}
