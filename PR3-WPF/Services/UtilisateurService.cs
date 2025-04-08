using Newtonsoft.Json;
using PR3_WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PR3_WPF.Services
{
    internal class UtilisateurService
    {
        private readonly HttpClient _httpClient;

        public UtilisateurService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Utilisateur>> GetAllUtilisateur()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5011/api/Utilisateurs");
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Utilisateur>>(data);
        }

        public async Task<(bool isSuccess, string token, Utilisateur utilisateur)> LoginAsync(LoginRequest loginRequest)
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

            return (false, null, null);
        }


    }
}
