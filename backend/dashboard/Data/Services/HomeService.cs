
using dashboard.Entities;
using Talabat.Core.Entities.Core;

namespace dashboard.Data.Services
{
    public class HomeService
    {
        private readonly HttpClient _httpClient;
        private readonly string baseURI = "https://localhost:7095/api/Admin/";
        public HomeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<dynamic> GetHomeDataAsync()
        {   
            return await _httpClient.GetFromJsonAsync<dynamic>(baseURI + $"home") ?? "null" ;
        }
    }
}
