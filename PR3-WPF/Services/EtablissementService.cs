using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PR3_WPF.Models;
using System.Net.Http.Headers;


namespace PR3_WPF.Services
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
            string jwtToken = _authService.ReadToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5011/api/Etablissements");
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Etablissement>>(data);
        }
    }
}
