using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorUI.Models;

namespace RazorUI.Pages.Groups
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public DeleteModel(IHttpClientFactory factory, IConfiguration config)
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
            var response = await _httpClient.DeleteAsync($"{_config["ApiBaseUrl"]}api/Group/{Group.Id}");
            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError(string.Empty, "Failed to delete group.");
            return Page();
        }
    }
}
