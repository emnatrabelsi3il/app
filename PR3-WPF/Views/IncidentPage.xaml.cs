using Newtonsoft.Json;
using PR3_WPF.Models;
using PR3_WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PR3_WPF.Views
{
    /// <summary>
    /// Logique d'interaction pour IncidentPage.xaml
    /// </summary>
    public partial class IncidentPage : Page
    {
        private const string apiIncident = "https://localhost:7011/api/Incidents";
        private const string apiSalle = "https://localhost:7011/api/Salles";
        private const string apiPoste = "https://localhost:7011/api/Postes";
        private const string apiEtablissement = "https://localhost:7011/api/Etablissements";
        public ObservableCollection<Incident> incidents { get; set; }
        private AuthService _authService;
        public IncidentPage()
        {
            InitializeComponent();
            incidents = new ObservableCollection<Incident>();

            incidentListView.ItemsSource = incidents;

            LoadDataFromApi();
        }

        private async void LoadDataFromApi()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    _authService = new AuthService(client);
                    string jwtToken = _authService.ReadToken();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);


                    HttpResponseMessage response = await client.GetAsync(apiIncident);

                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON data
                        var incidentList = JsonConvert.DeserializeObject<ObservableCollection<Incident>>(data);
                        incidents.Clear();
                        foreach (var incident in incidentList)
                        {
                            await SetIncidentTypeAndValue(incident, client);
                            incidents.Add(incident);
                            
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Error: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async Task SetIncidentTypeAndValue(Incident incident, HttpClient client)
        {
            if (incident.SalleId.HasValue)
            {
                incident.Type = "Salle";
                HttpResponseMessage response = await client.GetAsync($"{apiSalle}/{incident.SalleId.Value}");
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Salle>(data);
                    incident.Value = result.Numero;
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}");
                }
                //incident.value = await GetValueFromApi(apiSalle, incident.SalleId.Value, client, "Numero");
            }
            else if (incident.PosteId.HasValue)
            {
                incident.Type = "Poste";
                HttpResponseMessage response = await client.GetAsync($"{apiPoste}/{incident.PosteId.Value}");
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Poste>(data);
                    incident.Value = result.Numero;
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}");
                }
                //incident.value = await GetValueFromApi(apiPoste, incident.PosteId.Value, client, "Numero");
            }
            else if (incident.EtablissementId.HasValue)
            {
                incident.Type = "Etablissement";
                HttpResponseMessage response = await client.GetAsync($"{apiEtablissement}/{incident.EtablissementId.Value}");
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    // Deserialize the JSON response based on the expected key
                    var result = JsonConvert.DeserializeObject<Etablissement>(data);
                    incident.Value = result.Nom;
                }
                else
                {
                    MessageBox.Show($"Error: {response.StatusCode}");
                }
                //incident.value = await GetValueFromApi(apiEtablissement, incident.EtablissementId.Value, client, "Nom");
            }
        }

        private async Task<string> GetValueFromApi(string baseUrl, int id, HttpClient client, string valueKey)
        {
            HttpResponseMessage response = await client.GetAsync($"{baseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                // Deserialize the JSON response based on the expected key
                var result = JsonConvert.DeserializeObject<dynamic>(data);
                return result[valueKey];
            }
            else
            {
                MessageBox.Show($"Error fetching value from {baseUrl}/{id}: {response.StatusCode}");
                return string.Empty;
            }
        }
    }
}

