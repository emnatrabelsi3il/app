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

            posteListView.ItemsSource = Postes;

            LoadDataFromApi();
        }



        private async void LoadDataFromApi()
        {

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    _authService = new AuthService(client);
                    string jwtToken = _authService.ReadToken();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

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
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        // Handle unauthorized access
                        MessageBox.Show("Unauthorized access. Please check your credentials.");
                    }
                    else
                    {
                        MessageBox.Show($"Error: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }


    }
}
