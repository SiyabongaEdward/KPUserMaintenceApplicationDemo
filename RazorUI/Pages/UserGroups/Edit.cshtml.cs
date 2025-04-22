using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text;
using RazorUI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RazorUI.Pages.UserGroups
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;


        public EditModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _clientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [BindProperty]
        public UserGroupDto UserGroup { get; set; }

        public List<SelectListItem> Users { get; set; }
        public List<SelectListItem> Groups { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var client = _clientFactory.CreateClient("API");
            var apiBaseUrl = _configuration["ApiBaseUrl"];

            var userGroupResponse = await client.GetFromJsonAsync<UserGroupDto>($"{apiBaseUrl}api/UserGroup/{id}");
            var users = await client.GetFromJsonAsync<List<UserDto>>($"{apiBaseUrl}api/Users");
            var groups = await client.GetFromJsonAsync<List<GroupDto>>($"{apiBaseUrl}api/Group");

            if (userGroupResponse == null)
                return NotFound();

            UserGroup = userGroupResponse;

            Users = users.Select(u => new SelectListItem
            {
                Value = u.Id.ToString(),
                Text = $"{u.FirstName} {u.LastName}"
            }).ToList();

            Groups = groups.Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = g.Name
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (UserGroup.Id == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "Invalid UserGroup ID.");
                return Page();
            }

            var apiBaseUrl = _configuration["ApiBaseUrl"];
            var json = JsonSerializer.Serialize(UserGroup);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _clientFactory.CreateClient().GetAsync($"{apiBaseUrl}api/UserGroup/{UserGroup.Id}");

                if (response.IsSuccessStatusCode)
                {
                    var existingUserGroup = await response.Content.ReadFromJsonAsync<UserGroupDto>();

                    var deleteResponse = await _clientFactory.CreateClient().DeleteAsync($"{apiBaseUrl}api/UserGroup/{existingUserGroup.Id}");

                    if (deleteResponse.IsSuccessStatusCode)
                    {
                        var createResponse = await _clientFactory.CreateClient().PostAsync($"{apiBaseUrl}api/UserGroup", content);

                        if (createResponse.IsSuccessStatusCode)
                        {
                            return RedirectToPage("Index");
                        }
                        else
                        {
                            var errorMessage = await createResponse.Content.ReadAsStringAsync();
                            ModelState.AddModelError(string.Empty, "Failed to update user group.");
                        }
                    }
                    else
                    {
                        var deleteError = await deleteResponse.Content.ReadAsStringAsync();
                        ModelState.AddModelError(string.Empty, "Failed to delete existing user group.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User group not found.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error updating user group.");
            }

            return Page();
        }
    }
}
