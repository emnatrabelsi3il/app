using PR3_WPF.Services;  // Pour utiliser AuthService
using System;
using System.Net.Http;
using System.Windows;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using PR3_WPF.Models;
using PR3_WPF;

namespace PR3_WPF.Views
{
    public partial class LoginPage : Page
    {
        private readonly AuthService _authService;

        public class LoginResponse
        {
            public string Token { get; set; }
            public Utilisateur Utilisateur { get; set; }
        }

        public class Utilisateur
        {
            public int Id { get; set; }
            public string Login { get; set; }
            public string Nom { get; set; }
            public string Prenom { get; set; }
            public string Role { get; set; }
        }

        public LoginPage()
        {
            InitializeComponent();
            _authService = new AuthService(new HttpClient());
        }

        // Méthode pour gérer la connexion
        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = UserNameTextBox.Text;
            string password = PasswordBox.Password;

            // Vérifier que les champs ne sont pas vides
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }

            // Essayer de se connecter via l'API
            var loginSuccess = await TryLogin(username, password);
            if (loginSuccess)
            {
                // Enregistre le token et ouvre MainWindow
                //  var token = await _authService.GetToken();
                //_authService.StoreToken(token);  // Enregistrer le token dans un fichier

                MainWindow mainWindow = new MainWindow();
                Application.Current.MainWindow = mainWindow;
                mainWindow.Show();

                Window.GetWindow(this)?.Close();
            }
            else
            {
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.");
            }
        }

        // Méthode pour essayer de se connecter à l'API
        private async Task<bool> TryLogin(string username, string password)
        {
            try
            {
                var client = new HttpClient();

                // Créer un objet avec les informations de connexion
                var loginData = new { Login = username, MotDePasse = password };

                // Sérialiser l'objet en JSON
                var content = new StringContent(JsonConvert.SerializeObject(loginData), System.Text.Encoding.UTF8, "application/json");

                // Afficher les données envoyées pour débogage
                Console.WriteLine($"Login Data: {JsonConvert.SerializeObject(loginData)}");

                // Envoyer la requête POST à l'API pour l'authentification
                var response = await client.PostAsync("https://localhost:7011/api/Utilisateurs/login", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseData);

                    _authService.StoreToken(loginResponse.Token);
                    _authService.StoreRole(loginResponse.Utilisateur.Role);

                    MessageBox.Show("Connexion réussie !");

                    // Ouvre MainWindow et ferme LoginPage
                   // var mainWindow = new MainWindow();
                    //mainWindow.Show();  // Ouvre la fenêtre principale
                    //Window.GetWindow(this)?.Close();  // Ferme la fenêtre de connexion (LoginPage)

                    return true;
                }
                else
                {
                    // Si l'authentification échoue, afficher l'erreur
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {errorMessage}");

                    // Retourner false si l'authentification échoue
                    MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Afficher une erreur dans la console ou une fenêtre si nécessaire
                MessageBox.Show($"Erreur de connexion : {ex.Message}");
                return false;
            }
        }
    }
}