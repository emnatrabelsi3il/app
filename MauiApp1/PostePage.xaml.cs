namespace MauiApp1;

using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MauiApp1.Model;

public partial class PostePage : ContentPage
{
    public ObservableCollection<Poste> postes { get; set; } = new ObservableCollection<Poste>();

    private bool _alreadyLoaded = false;

    public PostePage()
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
                "https://hqx6q9wt-7011.uks1.devtunnels.ms/api/Postes"
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

            var poste = await response.Content.ReadFromJsonAsync<List<Poste>>();

            if (poste != null)
            {
                postes.Clear();

                foreach (var post in poste)
                {
                    postes.Add(post);
                }

                MyListView.ItemsSource = postes;
            }
            else
            {
                await DisplayAlert("Erreur", "Aucune donnée reçue depuis le serveur.", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur PostePage : {ex.Message}");
            await DisplayAlert("Erreur chargement", ex.Message, "OK");
        }
    }
}