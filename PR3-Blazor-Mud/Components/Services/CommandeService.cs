using Newtonsoft.Json;
using PR3_Blazor_Mud.Components.Models;
using System.Net.Http.Headers;

namespace PR3_Blazor_Mud.Components.Services
{
    public class CommandeService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;

        public CommandeService(HttpClient httpClient, AuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        private async Task AddAuthorizationHeader()
        {
            string? token = await _authService.GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Token introuvable.");
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task RefreshOnePoste(int posteId)
        {
            await AddAuthorizationHeader();

            var response = await _httpClient.PostAsync(
                $"https://localhost:7011/api/Commandes/poste/{posteId}/refresh",
                null
            );

            response.EnsureSuccessStatusCode();
        }

        public async Task RefreshAllPostes()
        {
            await AddAuthorizationHeader();

            var response = await _httpClient.PostAsync(
                "https://localhost:7011/api/Commandes/global/refresh",
                null
            );

            response.EnsureSuccessStatusCode();
        }

        public async Task<List<CommandePoste>> GetCommandes()
        {
            await AddAuthorizationHeader();

            var response = await _httpClient.GetAsync("https://localhost:7011/api/Commandes");

            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<CommandePoste>>(data)
                   ?? new List<CommandePoste>();
        }
    }
}