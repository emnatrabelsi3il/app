using PR3_Blazor.Components.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using System;

namespace PR3_Blazor.Components.Services
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
            string token =  _authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5011/api/Salles");
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Salle>>(data);
        }

        /**public async Task<List<Salle>> GetAllSalleByEtablissment()
        {
        }**/

        public async Task<Salle> GetSalleById(int salleId) {
            string token =  _authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5011/api/Salles/{salleId}");
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Salle>(data);
        
        }

        public async Task AddSalle(Salle salle)
        {
            string token =  _authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var salleFormated = JsonConvert.SerializeObject(salle);
            var content = new StringContent(salleFormated, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5011/api/Salles", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateSalle(Salle salle)
        {
            string token =  _authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var salleFormated = JsonConvert.SerializeObject(salle);
                var content = new StringContent(salleFormated, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"http://localhost:5011/api/Salles/{salle.Id}", content);
                response.EnsureSuccessStatusCode();

        }

        public async Task DeleteSalle(int salleId)
        {
            string token =  _authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"http://localhost:5011/api/Salles/{salleId}");
            response.EnsureSuccessStatusCode();

        }
    }

}
