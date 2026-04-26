using PR3_WPF.Services;
using PR3_WPF.Views;
using System.Net.Http;
using System.Windows;

namespace PR3_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var authService = new AuthService(new HttpClient());
            var token = authService.ReadToken();

            if (string.IsNullOrWhiteSpace(token))
            {
                navframe.Navigate(new LoginPage());
            }
            else
            {
                navframe.Navigate(new MainPage());
            }
        }

        private void Accueil_Click(object sender, RoutedEventArgs e)
        {
            navframe.Navigate(new MainPage());
        }

        private void Postes_Click(object sender, RoutedEventArgs e)
        {
            navframe.Navigate(new PostePage());
        }

        private void AjouterPoste_Click(object sender, RoutedEventArgs e)
        {
            navframe.Navigate(new MainPage());
        }

        private void Salles_Click(object sender, RoutedEventArgs e)
        {
            navframe.Navigate(new SallePage());
        }

        private void Etablissements_Click(object sender, RoutedEventArgs e)
        {
            navframe.Navigate(new EtablissementPage());
        }

        private void Incidents_Click(object sender, RoutedEventArgs e)
        {
            navframe.Navigate(new IncidentPage());
        }

        private void Utilisateurs_Click(object sender, RoutedEventArgs e)
        {
            navframe.Navigate(new UtilisateurPage());
        }
    }
}