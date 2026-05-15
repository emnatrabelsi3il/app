using Newtonsoft.Json;
using PR3_WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using PR3_WPF;
using PR3_WPF.Services;
using System.Net.Http.Headers;

namespace PR3_WPF.Views
{
    /// <summary>
    /// Logique d'interaction pour SallePage.xaml
    /// </summary>
    public partial class SallePage : Page
    {
        private const string apiSalle = "https://localhost:7011/api/Salles";
        public ObservableCollection<Salle> Salles { get; set; }
        private  AuthService _authService;


        public SallePage()
        {
            InitializeComponent();
            Salles = new ObservableCollection<Salle>();

            salleListView.ItemsSource = Salles;

            LoadDataFromApi();


        }

        private async void LoadDataFromApi()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    _authService = new AuthService(client);
                    string jwtToken = _authService.ReadToken()?.Trim();

                    if (string.IsNullOrWhiteSpace(jwtToken))
                    {
                        MessageBox.Show("Session expirée. Veuillez vous reconnecter.");
                        // Redirection vers la page de login
                        var loginPage = new LoginPage();
                        this.NavigationService.Navigate(loginPage);
                        return;
                    }

                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", jwtToken);

                    HttpResponseMessage response = await client.GetAsync(apiSalle);

                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();

                        var salleList = JsonConvert.DeserializeObject<ObservableCollection<Salle>>(data);
                        Salles.Clear();

                        foreach (var salle in salleList)
                        {
                            Salles.Add(salle);
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        MessageBox.Show("Accès non autorisé. Veuillez vous reconnecter.");
                        // Redirection vers la page de login
                        var loginPage = new LoginPage();
                        this.NavigationService.Navigate(loginPage);
                    }
                    else
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error: {response.StatusCode}\n{error}");
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
