using Newtonsoft.Json;
using PR3_WPF.Models;
using PR3_WPF.Services;
using PR3_WPF.Views;
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
    /// Logique d'interaction pour UtilisateurPage.xaml
    /// </summary>
    public partial class UtilisateurPage : Page
    {
        private const string apiUtilisateur = "https://localhost:7011/api/Utilisateurs";
        public ObservableCollection<Utilisateur> utilisateur { get; set; }
        private AuthService _authService;
        public UtilisateurPage()
        {
            InitializeComponent();
            utilisateur = new ObservableCollection<Utilisateur>();

            utilisateurListView.ItemsSource = utilisateur;

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

                    if (string.IsNullOrWhiteSpace(jwtToken))
                    {
                        MessageBox.Show("Session expirée. Veuillez vous reconnecter.");
                        return;
                    }

                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", jwtToken);

                    HttpResponseMessage response = await client.GetAsync(apiUtilisateur);

                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();

                        var UtilisateurList = JsonConvert.DeserializeObject<ObservableCollection<Utilisateur>>(data);
                        utilisateur.Clear();

                        foreach (var Utilisateur in UtilisateurList)
                        {
                            utilisateur.Add(Utilisateur);
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        MessageBox.Show("Accès non autorisé. Veuillez vous reconnecter.");
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

