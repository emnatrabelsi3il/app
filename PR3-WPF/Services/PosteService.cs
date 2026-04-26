using Newtonsoft.Json;
using PR3_WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PR3_WPF.Services
{
    public class PosteService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        public PosteService(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;

        }

        public async Task<List<Poste>> GetAllPoste()
        {
            string jwtToken = _authService.ReadToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:7011/api/Postes");
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Poste>>(data);
        }
    }
}
