using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorUI.Models;
using System.Text.Json;

namespace RazorUI.Pages.UserGroups
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public class GroupWithUsersDto
        {
            public string GroupName { get; set; }
            public List<UserGroupDto> Users { get; set; } = new();
        }
        public List<GroupWithUsersDto> GroupedUserGroups { get; set; } = new();
        public IndexModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
        }

        public async Task OnGetAsync()
        {
            var apiBaseUrl = _configuration["ApiBaseUrl"];

            var userGroupResponse = await _httpClient.GetAsync($"{apiBaseUrl}api/UserGroup");
            var userResponse = await _httpClient.GetAsync($"{apiBaseUrl}api/Users");
            var groupResponse = await _httpClient.GetAsync($"{apiBaseUrl}api/Group");

            if (userGroupResponse.IsSuccessStatusCode && userResponse.IsSuccessStatusCode && groupResponse.IsSuccessStatusCode)
            {
                var userGroups = await userGroupResponse.Content.ReadFromJsonAsync<List<UserGroupDto>>();
                var users = await userResponse.Content.ReadFromJsonAsync<List<UserDto>>();
                var groups = await groupResponse.Content.ReadFromJsonAsync<List<GroupDto>>();

                var enrichedUserGroups = userGroups.Select(ug =>
                {
                    var user = users.FirstOrDefault(u => u.Id == ug.UserId);
                    var group = groups.FirstOrDefault(g => g.Id == ug.GroupId);

                    return new UserGroupDto
                    {
                        Id = ug.Id,
                        FullName = user != null ? $"{user.FirstName} {user.LastName}" : "Unknown User",
                        GroupName = group?.Name ?? "Unknown Group"
                    };
                }).ToList();

                GroupedUserGroups = enrichedUserGroups
                    .GroupBy(g => g.GroupName)
                    .Select(g => new GroupWithUsersDto
                    {
                        GroupName = g.Key,
                        Users = g.ToList()
                    })
                    .ToList();
            }
        }
    }
}
