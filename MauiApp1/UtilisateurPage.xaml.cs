namespace MauiApp1;

using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MauiApp1.Model;

public partial class UtilisateurPage : ContentPage
{
    public ObservableCollection<Utilisateur> utilisateurs { get; set; } = new ObservableCollection<Utilisateur>();

    private bool _alreadyLoaded = false;

    public UtilisateurPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_alreadyLoaded)
            return;

        _alreadyLoaded = true;

        await LoadDataFromApiAsync();
    }

    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
    }

    private async Task LoadDataFromApiAsync()
    {
        try
        {
#if DEBUG
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
#else
            var handler = new HttpClientHandler();
#endif

            using var httpClient = new HttpClient(handler);

            var jwtToken = await SecureStorage.GetAsync("access_token");

            if (!string.IsNullOrWhiteSpace(jwtToken))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", jwtToken);
            }
            else
            {
                Console.WriteLine("Token absent dans SecureStorage.");
            }

            var response = await httpClient.GetAsync(
                "https://hqx6q9wt-7011.uks1.devtunnels.ms/api/Utilisateurs"
            );

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await DisplayAlert(
                    "Erreur authentification",
                    "L'API a refusé l'accès. Le token est absent, expiré ou invalide.",
                    "OK"
                );
                return;
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();

                await DisplayAlert(
                    "Erreur API",
                    $"Code : {response.StatusCode}\nDétail : {errorContent}",
                    "OK"
                );
                return;
            }

            var utilisateur = await response.Content.ReadFromJsonAsync<List<Utilisateur>>();

            if (utilisateur != null)
            {
                utilisateurs.Clear();

                foreach (var user in utilisateur)
                {
                    utilisateurs.Add(user);
                }

                MyListView.ItemsSource = utilisateurs;
            }
            else
            {
                await DisplayAlert("Erreur", "Aucune donnée reçue depuis le serveur.", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur UtilisateurPage : {ex.Message}");
            await DisplayAlert("Erreur chargement", ex.Message, "OK");
        }
    }
}