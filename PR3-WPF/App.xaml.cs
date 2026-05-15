using System;
using System.Linq;
using System.Net.Http;
using System.Windows;
using PR3_WPF.Services;
using PR3_WPF.Views;

namespace PR3_WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Créer une instance du service d'authentification
            var authService = new AuthService(new HttpClient());
            string token = authService.ReadToken();

            // Vérifie si le token existe et est valide
            if (string.IsNullOrWhiteSpace(token))
            {
                // Si le token est vide ou invalide, afficher LoginPage
                LoginPage loginPage = new LoginPage();  // Créer une instance de la page de login
                Window loginWindow = new Window
                {
                    Title = "Login",
                    Content = loginPage,
                    Width = 400,
                    Height = 500,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                loginWindow.Show();  // Afficher la page de connexion
            }
            else
            {
                // Si le token est valide, afficher MainWindow
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();  // Afficher la fenêtre principale
            }
        }

        // Ferme l'application et déconnecte l'utilisateur à la fermeture
        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            string macAdress = GetMacAddress();

            using (HttpClient client = new HttpClient())
            {
                var response = await client.PutAsync($"https://localhost:7011/api/Postes/DisconnectByMacAdress/{macAdress}", null);
                if (!response.IsSuccessStatusCode)
                {
                    // Gérer les erreurs de réponse (facultatif)
                }
            }
        }

        private string GetMacAddress()
        {
            var nic = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                                   .FirstOrDefault(n => n.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up);
            return nic?.GetPhysicalAddress().ToString() ?? "00:00:00:00:00:00";
        }
    }
}