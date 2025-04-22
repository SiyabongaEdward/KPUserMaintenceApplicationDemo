using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using RazorUI.Models;
using System.Net.Http;
using System.Text;

namespace RazorUI.Pages.Permissions
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        [BindProperty]
        public PermissionDto Permission { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            
            var client = _httpClientFactory.CreateClient("API");
            var response = await client.PostAsJsonAsync("api/permission", Permission);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to create permission.");
            return Page();
        }
    }
}
