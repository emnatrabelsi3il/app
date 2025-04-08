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
    /// Logique d'interaction pour EtablissementPage.xaml
    /// </summary>
    public partial class EtablissementPage : Page
    {
        private const string apiEtablissement = "http://localhost:5011/api/Etablissements";

        public ObservableCollection<Etablissement> etablissements { get; set; }
        private AuthService _authService;
        public EtablissementPage()
        {
            InitializeComponent();
            etablissements = new ObservableCollection<Etablissement>();

            etablissementListView.ItemsSource = etablissements;

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


                    HttpResponseMessage response = await client.GetAsync(apiEtablissement);

                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON data
                        var etablissementList = JsonConvert.DeserializeObject<ObservableCollection<Etablissement>>(data);
                        etablissements.Clear();
                        foreach (var etablissement in etablissementList)
                        {
                            etablissements.Add(etablissement);
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
    }
}
