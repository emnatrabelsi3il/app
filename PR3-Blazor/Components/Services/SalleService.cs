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
            string? token = await _authService.GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Token introuvable.");
            }

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
            string? token = await _authService.GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Token introuvable.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5011/api/Salles/{salleId}");
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Salle>(data);
        
        }

        public async Task AddSalle(Salle salle)
        {
            string? token = await _authService.GetTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5011/api/Salles");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(salle);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }
        }

        public async Task UpdateSalle(Salle salle)
        {
            string? token = await _authService.GetTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Put, $"http://localhost:5011/api/Salles/{salle.Id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(salle);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception(errorMessage);
            }
        }

        public async Task DeleteSalle(int salleId)
        {
            string? token = await _authService.GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Token introuvable.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"http://localhost:5011/api/Salles/{salleId}");
            response.EnsureSuccessStatusCode();

        }
    }

}
