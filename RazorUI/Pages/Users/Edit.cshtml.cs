using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using RazorUI.Models;

namespace RazorUI.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("API");
        }

        [BindProperty]
        public UserDto User { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/users/{id}");
                if (response.IsSuccessStatusCode)
                {
                    User = await response.Content.ReadFromJsonAsync<UserDto>();
                    return Page();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user: {ex.Message}");
                return RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/users/{User.Id}", User);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Index");
                }

                ModelState.AddModelError(string.Empty, "Failed to update user.");
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Something went wrong.");
                return Page();
            }
        }
    }
}
