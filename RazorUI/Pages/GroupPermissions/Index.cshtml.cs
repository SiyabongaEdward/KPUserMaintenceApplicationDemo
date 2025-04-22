using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using RazorUI.Models;

namespace RazorUI.Pages.GroupPermission
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IHttpClientFactory clientFactory, ILogger<IndexModel> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public List<GroupPermissionDto> GroupPermissions { get; set; }

        public async Task OnGetAsync()
        {
            var client = _clientFactory.CreateClient("API");

            try
            {
                var response = await client.GetAsync("api/grouppermission");

                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    GroupPermissions = await JsonSerializer.DeserializeAsync<List<GroupPermissionDto>>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    GroupPermissions = new List<GroupPermissionDto>();
                    _logger.LogError("Failed to get users. StatusCode: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling API");
                GroupPermissions = new List<GroupPermissionDto>();
            }
        }
    }
}
