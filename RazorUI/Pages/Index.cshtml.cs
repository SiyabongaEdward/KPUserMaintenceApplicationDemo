using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorUI.Models;
using System.Text.Json;

namespace RazorUI.Pages
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

        public List<UserDto> Users { get; set; }
        public int TotalUsers { get; set; }

        public async Task OnGetAsync()
        {
            var client = _clientFactory.CreateClient("API");

            try
            {
                var response = await client.GetAsync("api/users");

                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    Users = await JsonSerializer.DeserializeAsync<List<UserDto>>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    TotalUsers = Users?.Count ?? 0;
                }
                else
                {
                    Users = new List<UserDto>();
                    TotalUsers = 0;
                    _logger.LogError("Failed to get users. StatusCode: {StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling API");
                Users = new List<UserDto>();
                TotalUsers = 0;
            }
        }
    }
}
