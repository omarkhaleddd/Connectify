﻿using dashboard.Entities;

namespace dashboard.Data.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly string baseURI = "https://localhost:7095/api/Admin/";
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<User>>(baseURI + "Users");
        }
    }
}
