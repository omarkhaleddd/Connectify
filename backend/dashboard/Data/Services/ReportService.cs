using dashboard.Entities;
using Talabat.Core.Entities.Core;
namespace dashboard.Data.Services
{
    public class ReportService
    {
        private readonly HttpClient _httpClient;
        private readonly string baseURI = "https://localhost:7095/api/Admin/";
        public ReportService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<ReportedPost>> GetReportedPostsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ReportedPost>>(baseURI) ?? new List<ReportedPost>();
        }
        public async Task<string> DismissReport(int id){
             HttpResponseMessage response = await _httpClient.PutAsJsonAsync(baseURI+$"action-post/{id}",new {number = 1});
             return "post dismissed";
        }
        public async Task<string> ResolveReport(int id){
             HttpResponseMessage response = await _httpClient.PutAsJsonAsync(baseURI+$"action-post/{id}",new {number = 0});
             return "post resolved";
        }
    }
    }