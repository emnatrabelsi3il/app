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
    public class SalleService
    {

        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        public SalleService(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }
        public async Task<List<Salle>> GetAllSalle()
        {

            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5011/api/Salles");
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Salle>>(data);
        }
    }
}
