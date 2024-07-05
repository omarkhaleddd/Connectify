
using dashboard.Entities;
using Talabat.Core.Entities.Core;

namespace dashboard.Data.Services
{
    public class PostService
    {
        private readonly HttpClient _httpClient;
        private readonly string baseURI = "https://localhost:7095/api/Admin/";
        public PostService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(string id)
        {   
            return await _httpClient.GetFromJsonAsync<IEnumerable<Post>>(baseURI + $"GetPostsByUserId/{id}") ?? new List<Post>();
        }
    }
}
