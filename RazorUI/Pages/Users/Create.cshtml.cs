using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorUI.Models;

namespace RazorUI.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public UserDto UserDto { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var client = _httpClientFactory.CreateClient("API");
            var response = await client.PostAsJsonAsync("api/users", UserDto);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("/Index");

            ModelState.AddModelError(string.Empty, "Failed to create user");
            return Page();
        }

    }
}
