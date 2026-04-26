using Newtonsoft.Json;
using PR3_Blazor_Mud.Components.Models;
using System.Net.Http.Headers;
using System.Text;

namespace PR3_Blazor_Mud.Components.Services
{
    public class IncidentService
{

    private readonly HttpClient _httpClient;
    private readonly AuthService _authService;

    public IncidentService(HttpClient httpClient, AuthService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    public async Task<List<Incident>> GetAllIncident()
    {
            string? token = await _authService.GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Token introuvable.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:7011/api/Incidents");
        response.EnsureSuccessStatusCode();

        string data = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<Incident>>(data);
    }

    public async Task<Incident> GetIncidentById(int incidentId)
    {
            string? token = await _authService.GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Token introuvable.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"https://localhost:7011/api/Incidents/{incidentId}");
        response.EnsureSuccessStatusCode();

        string data = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Incident>(data);

    }

    public async Task AddIncident(Incident incident)
    {
            string? token = await _authService.GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Token introuvable.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var incidentFormated = JsonConvert.SerializeObject(incident);
        var content = new StringContent(incidentFormated, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("https://localhost:7011/api/Incidents", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateIncident(Incident incident)
    {
            string? token = await _authService.GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Token introuvable.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var incidentFormated = JsonConvert.SerializeObject(incident);
        var content = new StringContent(incidentFormated, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"https://localhost:7011/api/Incidents/{incident.Id}", content);
        response.EnsureSuccessStatusCode();

    }

    public async Task DeleteIncident(int incidentId)
    {
            string? token = await _authService.GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("Token introuvable.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"https://localhost:7011/api/Incidents/{incidentId}");
        response.EnsureSuccessStatusCode();

    }

}
}
