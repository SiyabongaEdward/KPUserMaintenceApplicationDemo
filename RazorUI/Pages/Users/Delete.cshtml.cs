using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorUI.Models;

namespace RazorUI.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public DeleteModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        [BindProperty]
        public UserDto User { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var apiBaseUrl = _configuration["ApiBaseUrl"];
            var response = await _httpClient.GetAsync($"{apiBaseUrl}api/users/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Index");
            }

            User = await response.Content.ReadFromJsonAsync<UserDto>();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var apiBaseUrl = _configuration["ApiBaseUrl"];
            var response = await _httpClient.DeleteAsync($"{apiBaseUrl}api/users/{User.Id}");

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Failed to delete user.");
                return Page();
            }

            return RedirectToPage("/Index");
        }
    }
}
