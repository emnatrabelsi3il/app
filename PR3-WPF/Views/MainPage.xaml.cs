using Newtonsoft.Json;
using PR3_WPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
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
using PR3_WPF.Services;
using System.Diagnostics;
using System.IO;
using System.Windows.Threading;

namespace PR3_WPF.Views
{
    /// <summary>
    /// Logique d'interaction pour MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {

        public List<Salle> Salles { get; set; }
        public Poste poste { get; set; }
        public MainPage()
        {
            InitializeComponent();

            var authService = new AuthService(new HttpClient());
            var token = authService.ReadToken();  // Lire le token

            // Vérifie si le token existe et si l'utilisateur est authentifié
            if (string.IsNullOrEmpty(token))
            {
                // Si le token est vide ou invalide, redirige vers la page de login
                this.NavigationService.Navigate(new LoginPage());
            }
            else
            {
                // Authentification réussie, charger les données et informations système
                LoadData();
                LoadSystemInformation();

                // Vérifie si l'utilisateur est un admin
                if (IsUserAdmin(authService))
                {
                    EnableEditing(true);  // Activer l'édition si l'utilisateur est un admin
                }
                else
                {
                    EnableEditing(false);  // Désactiver l'édition pour les non-admins
                }
            }
        }

        // Méthode pour vérifier si l'utilisateur est un admin
        private bool IsUserAdmin(AuthService authService)
        {
            var role = authService.GetUserRole();  // Récupère le rôle de l'utilisateur
            MessageBox.Show($"Role: {role}");  // Affiche le rôle de l'utilisateur pour débogage
            return role == "admin";  // Retourne true si l'utilisateur est un admin
        }

        // Méthode pour activer/désactiver les champs d'édition
        private void EnableEditing(bool isAdmin)
        {
            NameTextBox.IsReadOnly = !isAdmin;  // Le champ "Nom" est modifiable uniquement pour les admins
            RoomComboBox.IsEnabled = isAdmin;   // La salle est modifiable uniquement pour les admins
            RegisterButton.IsEnabled = isAdmin; // Le bouton d'ajout est activé uniquement pour les admins
        }
    

        private async Task LoadData()
        {
            poste = new Poste();

            Salles = new List<Salle>();
            string macAdress  = GetMacAddress();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage responseExist = await client.GetAsync($"https://localhost:7011/api/Postes/ByMacAdress/{macAdress}");
                    if (responseExist.IsSuccessStatusCode)
                    {
                        string data = await responseExist.Content.ReadAsStringAsync();
                        var poste = JsonConvert.DeserializeObject<Poste>(data);

                        if (poste != null)
                        {
                            NameTextBox.Text = poste.Numero;
                            NameTextBox.IsReadOnly = true;
                            MacAddressTextBox.IsReadOnly = true;
                            HttpResponseMessage responseSalle = await client.GetAsync($"https://localhost:7011/api/Salles/{poste.SalleId}");
                            if (responseSalle.IsSuccessStatusCode)
                            {
                                string salleData = await responseSalle.Content.ReadAsStringAsync();
                                var salle = JsonConvert.DeserializeObject<Salle>(salleData);

                                RoomComboBox.ItemsSource = new List<Salle> { salle };
                                RoomComboBox.DisplayMemberPath = "Numero";
                                RoomComboBox.SelectedItem = salle;
                                RoomComboBox.IsEnabled = false;
                            }
                        }

                    }
                    else
                    {
                        HttpResponseMessage response = await client.GetAsync("https://localhost:7011/api/Salles");

                        if (response.IsSuccessStatusCode)
                        {
                            string data = await response.Content.ReadAsStringAsync();

                            // Deserialize the JSON data
                            var salleList = JsonConvert.DeserializeObject<ObservableCollection<Salle>>(data);
                            Salles.Clear();
                            foreach (var poste in salleList)
                            {
                                Salles.Add(poste);
                            }
                            RoomComboBox.ItemsSource = Salles;
                            RoomComboBox.DisplayMemberPath = "Numero";

                        }
                        else
                        {
                            MessageBox.Show($"Error: {response.StatusCode}");
                        }
                    }                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            //RoomComboBox.ItemsSource = Postes;
            //RoomComboBox.DisplayMemberPath = "Numero";
        }

        private string GetMacAddress()
        {
            var nic = NetworkInterface.GetAllNetworkInterfaces()
                                      .FirstOrDefault(n => n.OperationalStatus == OperationalStatus.Up);
            return nic?.GetPhysicalAddress().ToString() ?? "00:00:00:00:00:00";
        }

        private void LoadSystemInformation()
        {
            var nic = NetworkInterface.GetAllNetworkInterfaces()
                              .FirstOrDefault(n => n.OperationalStatus == OperationalStatus.Up);
            MacAddressTextBox.Text = nic?.GetPhysicalAddress().ToString() ?? "00:00:00:00:00";

            // Get RAM usage
            var ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            float availableRam = ramCounter.NextValue();
            RamUsageTextBox.Text = $"{availableRam} MB available";

            // Get Operating System
            OperatingSystemTextBox.Text = Environment.OSVersion.ToString();

            // Set up for Storage Info ProgressBar
            StorageInfoPanel.Children.Clear(); // Assuming StorageInfoPanel is the container for ProgressBar

            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    // Create a text block for displaying partition name and usage
                    double usedSpacePercentage = (double)(drive.TotalSize - drive.TotalFreeSpace) / drive.TotalSize * 100.0;

                    var textBlock = new TextBlock();
                    textBlock.Text = $"{drive.Name} - {usedSpacePercentage:F2}%";

                    // Create a StackPanel to hold both text block and progress bar horizontally
                    var stackPanel = new StackPanel();
                    stackPanel.Orientation = Orientation.Horizontal;
                    stackPanel.Children.Add(textBlock);

                    // Create the progress bar
                    var progressBar = new ProgressBar();
                    progressBar.Value = 100 - usedSpacePercentage; // Value is set to remaining space percentage

                    // Additional text showing used and free space
                    double usedSpaceGB = (double)(drive.TotalSize - drive.TotalFreeSpace) / (1024 * 1024 * 1024);
                    double totalSpaceGB = (double)drive.TotalSize / (1024 * 1024 * 1024);
                    progressBar.ToolTip = $"{usedSpaceGB:F2} GB used of {totalSpaceGB:F2} GB";

                    // Customizing the appearance of the ProgressBar
                    progressBar.Height = 30; // Increase the height for a thicker appearance
                    progressBar.Margin = new Thickness(0, 5, 0, 5); // Add margin for spacing

                    // Add progress bar to the second column of the grid
                    Grid.SetColumn(progressBar, 1);

                    // Add text block (partition info) to the first column of the grid
                    Grid.SetColumn(stackPanel, 0);

                    // Add stack panel to the StorageInfoPanel (which is a Grid)
                    StorageInfoPanel.Children.Add(stackPanel);
                    StorageInfoPanel.Children.Add(progressBar);
                }
            }

            // Refresh RAM usage every 0.5 seconds
            DispatcherTimer ramTimer = new DispatcherTimer();
            ramTimer.Interval = TimeSpan.FromSeconds(0.5);
            ramTimer.Tick += (sender, args) =>
            {
                availableRam = ramCounter.NextValue();
                RamUsageTextBox.Text = $"{availableRam} MB available";
            };
            ramTimer.Start();
        }

        private async void OnRegisterButtonClick(object sender, RoutedEventArgs e)
        {
            var authService = new AuthService(new HttpClient());

            if (!IsUserAdmin(authService))
            {
                MessageBox.Show("Seuls les administrateurs peuvent ajouter ou modifier des postes.");
                return;
            }

            var name = NameTextBox.Text;
            var macAddress = MacAddressTextBox.Text;
            var selectedSalle = RoomComboBox.SelectedItem as Salle;

            if (string.IsNullOrWhiteSpace(name) || selectedSalle == null)
            {
                MessageBox.Show("Veuillez remplir le nom et sélectionner une salle.");
                return;
            }

            Poste poste = new Poste
            {
                Numero = name,
                MacAdress = macAddress,
                SalleId = selectedSalle.Id,
                IsConnected = true
            };

            var json = JsonConvert.SerializeObject(poste);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                try
                {
                    // 🔍 Vérifier si le poste existe déjà
                    var checkResponse = await client.GetAsync($"https://localhost:7011/api/Postes/ByMacAdress/{macAddress}");

                    if (checkResponse.IsSuccessStatusCode)
                    {
                        // 👉 CAS : le poste existe déjà
                        MessageBox.Show("Ce poste existe déjà. La modification se fait depuis l'application Blazor.");
                        return;
                    }

                    // 👉 CAS : le poste n'existe pas → création
                    var response = await client.PostAsync("https://localhost:7011/api/Postes", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Enregistrement réussi !");
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Erreur lors de l'enregistrement:\n{error}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur : {ex.Message}");
                }
            }
        }
        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new LoginPage());
        }
    }
}
