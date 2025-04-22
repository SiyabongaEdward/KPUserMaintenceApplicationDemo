using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorUI.Models;

namespace RazorUI.Pages.Groups
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public CreateModel(IHttpClientFactory factory, IConfiguration config)
        {
            _httpClient = factory.CreateClient();
            _config = config;
        }

        [BindProperty]
        public GroupDto Group { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var response = await _httpClient.PostAsJsonAsync($"{_config["ApiBaseUrl"]}api/Group", Group);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("Index");

            ModelState.AddModelError(string.Empty, "Failed to create group.");
            return Page();
        }
    }
}
