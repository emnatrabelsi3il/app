using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
using PR3_WPF.Services;
using PR3_WPF.Models;



namespace PR3_WPF.Views
{
    /// <summary>
    /// Logique d'interaction pour LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private readonly UtilisateurService utilisateurService;
        private readonly AuthService authService;
        public LoginPage()
        {
            InitializeComponent();
            HttpClient httpClient = new HttpClient();
            utilisateurService = new UtilisateurService(httpClient);
            authService = new AuthService(httpClient);
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string username = UserNameTextBox.Text;
            string password = PasswordBox.Password;

            var loginRequest = new LoginRequest
            {
                Login = username,
                MotDePasse = password
            };

            var result = await utilisateurService.LoginAsync(loginRequest);
            var token = result.token;
            var utilisateur = result.utilisateur;

            authService.StoreToken(token);

            MainWindow mainWindow = new MainWindow
            {
                Width = this.Width,
                Height = this.Height
            };

            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            Window.GetWindow(this).Close();
            /**if (await utilisateurService.UserExist(username, password))
            {
                MainWindow mainWindow = new MainWindow
                {
                    Width = this.Width,
                    Height = this.Height
                };

                Application.Current.MainWindow = mainWindow;
                mainWindow.Show();
                Window.GetWindow(this).Close();
            }
            else
            {
                // Handle failure
                MessageBox.Show("Login failed. Please try again.");
            }**/


        }
    }
}
