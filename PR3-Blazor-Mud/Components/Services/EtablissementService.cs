using Newtonsoft.Json;
using PR3_Blazor_Mud.Components.Models;
using System.Net.Http.Headers;
using System.Text;

namespace PR3_Blazor_Mud.Components.Services
{
    public class EtablissementService
{
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        public EtablissementService(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        public async Task<List<Etablissement>> GetAllEtablissement()
        {
            string? token = await _authService.GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Token introuvable.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:7011/api/Etablissements");
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Etablissement>>(data);
        }

        public async Task<Etablissement> GetEtablissementById(int etablissementId)
        {
            string? token = await _authService.GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Token introuvable.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7011/api/Etablissements/{etablissementId}");
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Etablissement>(data);

        }

        public async Task AddEtablissement(Etablissement etablissement)
        {
            string? token = await _authService.GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Token introuvable.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var etablissementFormated = JsonConvert.SerializeObject(etablissement);
            var content = new StringContent(etablissementFormated, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7011/api/Etablissements", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateEtablissement(Etablissement etablissement)
        {
            string? token = await _authService.GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Token introuvable.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var etablissementFormated = JsonConvert.SerializeObject(etablissement);
            var content = new StringContent(etablissementFormated, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"https://localhost:7011/api/Etablissements/{etablissement.Id}", content);
            response.EnsureSuccessStatusCode();

        }

        public async Task DeleteEtablissement(int etablissementId)
        {
            string? token = await _authService.GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Token introuvable.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"https://localhost:7011/api/Etablissements/{etablissementId}");
            response.EnsureSuccessStatusCode();

        }
    }
}
