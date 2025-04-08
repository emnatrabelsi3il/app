using Newtonsoft.Json;
using PR3_Blazor.Components.Models;
using System.Net.Http.Headers;
using System.Text;

namespace PR3_Blazor.Components.Services
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
            string token =  _authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5011/api/Etablissements");
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Etablissement>>(data);
        }

        public async Task<Etablissement> GetEtablissementById(int etablissementId)
        {
            string token =  _authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5011/api/Etablissements/{etablissementId}");
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Etablissement>(data);

        }

        public async Task AddEtablissement(Etablissement etablissement)
        {
            string token =  _authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var etablissementFormated = JsonConvert.SerializeObject(etablissement);
            var content = new StringContent(etablissementFormated, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5011/api/Etablissements", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateEtablissement(Etablissement etablissement)
        {
            string token =  _authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var etablissementFormated = JsonConvert.SerializeObject(etablissement);
            var content = new StringContent(etablissementFormated, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"http://localhost:5011/api/Etablissements/{etablissement.Id}", content);
            response.EnsureSuccessStatusCode();

        }

        public async Task DeleteEtablissement(int etablissementId)
        {
            string token =  _authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"http://localhost:5011/api/Etablissements/{etablissementId}");
            response.EnsureSuccessStatusCode();

        }
    }
}
