using Newtonsoft.Json;
using PR3_Blazor.Components.Models;
using System.Text;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Rewrite;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.JSInterop;

namespace PR3_Blazor.Components.Services
{
    public class UtilisateurService
{
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;
        private readonly AuthService _authService;

        public UtilisateurService(HttpClient httpClient, IJSRuntime jsRuntime, AuthService authService)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            _authService = authService;
        }
        public async Task<(bool isSuccess,string token, Utilisateur utilisateur)> LoginAsync(LoginRequest loginRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5011/api/utilisateurs/login", loginRequest);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response JSON: {jsonString}");

                var result = System.Text.Json.JsonSerializer.Deserialize<LoginResponse>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (result != null)
                {
                    return (true, result.Token, result.Utilisateur);
                }
            }

            return (false,null, null);
        }
        public async Task<List<Utilisateur>> GetAllUtilisateur()
        {
            string token =  _authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5011/api/Utilisateurs");
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Utilisateur>>(data);
        }

        public async Task<Utilisateur> GetUtilisateurById(int utilisateurId)
        {
            string token =  _authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5011/api/Utilisateurs/{utilisateurId}");
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Utilisateur>(data);

        }

        public async Task<Utilisateur> GetUtilisateurByLogin(string login)
        {
            string token =_authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5011/api/Utilisateurs/login/{login}");
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Utilisateur>(data);
        }
        public async Task AddUtilisateur(Utilisateur utilisateur) 
        {
            utilisateur.Role = "user";
            string token =  _authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var UtilisateurFormated = JsonConvert.SerializeObject(utilisateur);
            var content = new StringContent(UtilisateurFormated, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5011/api/Utilisateurs/Create", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateUtilisateur(Utilisateur utilisateur)
        {
            string token =  _authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var UtilisateurFormated = JsonConvert.SerializeObject(utilisateur);
            var content = new StringContent(UtilisateurFormated, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"http://localhost:5011/api/Utilisateurs/{utilisateur.Id}", content);
            response.EnsureSuccessStatusCode();

        }

        public async Task DeleteUtilisateur(int utilisateurId)
        {
            string token =  _authService.GetTokenFromSessionAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"http://localhost:5011/api/Utilisateurs/{utilisateurId}");
            response.EnsureSuccessStatusCode();

        }

    }
}

