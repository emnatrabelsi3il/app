using Newtonsoft.Json;
using PR3_Blazor.Components.Models;
using System.Net.Http.Headers;
using System.Text;

namespace PR3_Blazor.Components.Services
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
            string token =  _authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5011/api/Postes");
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Poste>>(data);
        }

        public async Task<Poste> GetPosteById(int posteId)
        {
            string token =  _authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5011/api/Postes/{posteId}");
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Poste>(data);

        }

        public async Task AddPoste(Poste poste)
        {
            string token =  _authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var posteFormated = JsonConvert.SerializeObject(poste);
            var content = new StringContent(posteFormated, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5011/api/Postes", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdatePoste(Poste poste)
        {
            string token =  _authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var posteFormated = JsonConvert.SerializeObject(poste);
            var content = new StringContent(posteFormated, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"http://localhost:5011/api/Postes/{poste.Id}", content);
            response.EnsureSuccessStatusCode();

        }

        public async Task DeletePoste(int posteId)
        {
            string token =  _authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"http://localhost:5011/api/Postes/{posteId}");
            response.EnsureSuccessStatusCode();

        }
    }
}
