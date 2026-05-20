using MauiApp1.Model;
using System.Net.Http.Json;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace MauiApp1
{
    public partial class AppShell : Shell
    {
        private bool _alreadyLoggedIn = false;

        public AppShell()
        {
            InitializeComponent();

            Loaded += async (sender, e) =>
            {
                if (_alreadyLoggedIn)
                    return;

                _alreadyLoggedIn = true;

                await AutoLoginAsync();
            };
        }

        private async Task AutoLoginAsync()
        {
            try
            {
                await DisplayAlert("DEBUG", "AutoLogin lancé", "OK");

                SecureStorage.Default.Remove("access_token");

                var loginRequest = new LoginRequest
                {
                    Login = "adam",
                    MotDePasse = "admin123"
                };

                var result = await LoginAsync(loginRequest);

                if (!result.isSuccess || string.IsNullOrWhiteSpace(result.token))
                {
                    await DisplayAlert(
                        "Erreur",
                        "Connexion automatique échouée. Aucun token reçu.",
                        "OK"
                    );
                    return;
                }

                await SecureStorage.Default.SetAsync("access_token", result.token);

                var savedToken = await SecureStorage.Default.GetAsync("access_token");

                await DisplayAlert(
                    "Succès",
                    $"Token enregistré.\nLongueur : {savedToken?.Length}",
                    "OK"
                );
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Erreur AutoLogin",
                    ex.Message,
                    "OK"
                );
            }
        }

        private async Task<(bool isSuccess, string? token, Utilisateur? utilisateur)> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
#if DEBUG
                var handler = new SocketsHttpHandler
                {
                    SslOptions = new System.Net.Security.SslClientAuthenticationOptions
                    {
                        RemoteCertificateValidationCallback =
                            (sender, certificate, chain, sslPolicyErrors) => true
                    }
                };
#else
        var handler = new SocketsHttpHandler();
#endif

                using var client = new HttpClient(handler);

                var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    "https://hqx6q9wt-7011.uks1.devtunnels.ms/api/utilisateurs/login"
                );

                request.Version = new Version(1, 1);
                request.VersionPolicy = HttpVersionPolicy.RequestVersionExact;
                request.Content = JsonContent.Create(loginRequest);

                var response = await client.SendAsync(request);

                var jsonString = await response.Content.ReadAsStringAsync();

                await DisplayAlert(
                    "DEBUG Login",
                    $"Status : {response.StatusCode}\nRéponse : {jsonString}",
                    "OK"
                );

                if (!response.IsSuccessStatusCode)
                {
                    return (false, null, null);
                }

                var result = JsonSerializer.Deserialize<LoginResponse>(
                    jsonString,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

                if (result == null || string.IsNullOrWhiteSpace(result.Token))
                {
                    await DisplayAlert(
                        "Erreur Login",
                        "La réponse API ne contient pas de token.",
                        "OK"
                    );

                    return (false, null, null);
                }

                return (true, result.Token, result.Utilisateur);
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Exception LoginAsync",
                    ex.ToString(),
                    "OK"
                );

                return (false, null, null);
            }
        }
    }
}