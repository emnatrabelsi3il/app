using Newtonsoft.Json;
using PR3_WPF.Models;
using PR3_WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;

namespace PR3_WPF.Views
{
    /// <summary>
    /// Logique d'interaction pour PostePage.xaml
    /// </summary>
    public partial class PostePage : Page
    {
        private const string apiPoste = "https://localhost:7011/api/Postes/details";
        public ObservableCollection<Poste> Postes { get; set; }

        private  AuthService _authService;

        public PostePage()
        {

            InitializeComponent();
            Postes = new ObservableCollection<Poste>();

            PostesListView.ItemsSource = Postes;

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

                    HttpResponseMessage response = await client.GetAsync(apiPoste);

                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();

                        var posteList = JsonConvert.DeserializeObject<ObservableCollection<Poste>>(data);
                        Postes.Clear();

                        foreach (var poste in posteList)
                        {
                            Postes.Add(poste);
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
        private async void RefreshSelectedPoste_Click(object sender, RoutedEventArgs e)
        {
            var selectedPoste = PostesListView.SelectedItem as Poste;

            if (selectedPoste == null)
            {
                MessageBox.Show("Veuillez sélectionner un poste.");
                return;
            }

            await SendRefreshCommandForOnePoste(selectedPoste.Id);
        }

        private async void RefreshAllPostes_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Voulez-vous vraiment actualiser les informations de tous les postes ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            await SendRefreshCommandForAllPostes();
        }

        private async Task SendRefreshCommandForOnePoste(int posteId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var token = _authService.ReadToken();

                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                    var response = await client.PostAsync(
                        "https://localhost:7011/api/Commandes/poste/" + posteId + "/refresh",
                        null
                    );

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Commande envoyée au poste sélectionné.");
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Erreur : " + error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'envoi de la commande : " + ex.Message);
            }
        }

        private async Task SendRefreshCommandForAllPostes()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var token = _authService.ReadToken();

                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);

                    var response = await client.PostAsync(
                        "https://localhost:7011/api/Commandes/global/refresh",
                        null
                    );

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Commande globale envoyée à tous les postes.");
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Erreur : " + error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'envoi de la commande globale : " + ex.Message);
            }
        }


    }
}
