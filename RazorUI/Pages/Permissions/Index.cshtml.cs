using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorUI.Models;
using System.Text.Json;

namespace RazorUI.Pages.Permissions
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

        public List<PermissionDto> Permissions { get; set; }

        public async Task OnGetAsync()
        {
            var client = _clientFactory.CreateClient("API");

            try
            {
                var response = await client.GetAsync("api/Permission");

                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    Permissions = await JsonSerializer.DeserializeAsync<List<PermissionDto>>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    Permissions = new List<PermissionDto>();
                    _logger.LogError("Failed to get users. StatusCode: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling API");
                Permissions = new List<PermissionDto>();
            }
        }
    }
}
