using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorUI.Models;

namespace RazorUI.Pages.Groups
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public IndexModel(IHttpClientFactory factory, IConfiguration config)
        {
            _httpClient = factory.CreateClient();
            _config = config;
        }

        public List<GroupDto> Groups { get; set; } = new();

        public async Task OnGetAsync()
        {
            var response = await _httpClient.GetAsync($"{_config["ApiBaseUrl"]}api/Group");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<GroupDto>>();
                if (result != null)
                    Groups = result;
            }
        }
    }
}
