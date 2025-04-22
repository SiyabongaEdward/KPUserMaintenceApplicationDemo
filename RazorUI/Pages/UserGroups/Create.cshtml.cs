using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorUI.Models;
using System.Text;
using System.Text.Json;

namespace RazorUI.Pages.UserGroups
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        [BindProperty]
        public UserGroupDto UserGroup { get; set; } = new();

        public CreateModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var users = await _httpClient.GetFromJsonAsync<List<UserDto>>("http://localhost:5298/api/users");
            var groups = await _httpClient.GetFromJsonAsync<List<GroupDto>>("http://localhost:5298/api/group");

            UserGroup = new UserGroupDto
            {
                Users = users.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = $"{u.FirstName} {u.LastName}" }).ToList(),
                Groups = groups.Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name }).ToList()
            };

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var apiBaseUrl = _configuration["ApiBaseUrl"];
            var json = JsonSerializer.Serialize(UserGroup);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{apiBaseUrl}api/UserGroup", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "Failed to assign user to group.");
            return Page();
        }
    }
}
