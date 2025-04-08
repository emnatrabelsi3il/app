using Newtonsoft.Json;
using PR3_Blazor.Components.Models;
using System.Net.Http.Headers;
using System.Text;

namespace PR3_Blazor.Components.Services
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
        string token = _authService.GetTokenFromSessionAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5011/api/Incidents");
        response.EnsureSuccessStatusCode();

        string data = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<List<Incident>>(data);
    }

    public async Task<Incident> GetIncidentById(int incidentId)
    {
        string token = _authService.GetTokenFromSessionAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _httpClient.GetAsync($"http://localhost:5011/api/Incidents/{incidentId}");
        response.EnsureSuccessStatusCode();

        string data = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Incident>(data);

    }

    public async Task AddIncident(Incident incident)
    {
        string token = _authService.GetTokenFromSessionAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var incidentFormated = JsonConvert.SerializeObject(incident);
        var content = new StringContent(incidentFormated, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("http://localhost:5011/api/Incidents", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateIncident(Incident incident)
    {
        string token = _authService.GetTokenFromSessionAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var incidentFormated = JsonConvert.SerializeObject(incident);
        var content = new StringContent(incidentFormated, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"http://localhost:5011/api/Incidents/{incident.Id}", content);
        response.EnsureSuccessStatusCode();

    }

    public async Task DeleteIncident(int incidentId)
    {
        string token = _authService.GetTokenFromSessionAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.DeleteAsync($"http://localhost:5011/api/Incidents/{incidentId}");
        response.EnsureSuccessStatusCode();

    }

}
}
