namespace MauiApp1;

using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MauiApp1.Model;

public partial class PostePage : ContentPage
{
    public ObservableCollection<Poste> Postes { get; set; } = new ObservableCollection<Poste>();

    public PostePage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadDataFromApiAsync();
    }

    private HttpClient CreateHttpClient()
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

        return new HttpClient(handler);
    }

    private async Task AddTokenAsync(HttpClient httpClient)
    {
        var jwtToken = await SecureStorage.GetAsync("access_token");

        if (!string.IsNullOrWhiteSpace(jwtToken))
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", jwtToken);
        }
    }

    private async Task LoadDataFromApiAsync()
    {
        try
        {
            using var httpClient = CreateHttpClient();

            await AddTokenAsync(httpClient);

            var response = await httpClient.GetAsync(
                $"{ApiConfig.BaseUrl}/api/Postes/details"
            );

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await DisplayAlert(
                    "Erreur authentification",
                    "Le token est absent, expiré ou invalide.",
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

            var posteList = await response.Content.ReadFromJsonAsync<List<Poste>>();

            Postes.Clear();

            if (posteList != null)
            {
                foreach (var poste in posteList)
                {
                    Postes.Add(poste);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur chargement", ex.Message, "OK");
        }
    }

    private async void OnReloadClicked(object sender, EventArgs e)
    {
        await LoadDataFromApiAsync();
    }

    private async void OnRefreshOneClicked(object sender, EventArgs e)
    {
        try
        {
            if (sender is not Button button || button.CommandParameter == null)
            {
                await DisplayAlert("Erreur", "Poste introuvable.", "OK");
                return;
            }

            int posteId = Convert.ToInt32(button.CommandParameter);

            using var httpClient = CreateHttpClient();

            await AddTokenAsync(httpClient);

            var response = await httpClient.PostAsync(
                $"{ApiConfig.BaseUrl}/api/Commandes/poste/{posteId}/refresh",
                null
            );

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert(
                    "Succès",
                    "Commande envoyée au poste sélectionné.",
                    "OK"
                );

                await LoadDataFromApiAsync();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();

                await DisplayAlert(
                    "Erreur commande",
                    $"Code : {response.StatusCode}\nDétail : {error}",
                    "OK"
                );
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", ex.Message, "OK");
        }
    }

    private async void OnRefreshAllClicked(object sender, EventArgs e)
    {
        try
        {
            bool confirm = await DisplayAlert(
                "Confirmation",
                "Voulez-vous actualiser tous les postes ?",
                "Oui",
                "Non"
            );

            if (!confirm)
                return;

            using var httpClient = CreateHttpClient();

            await AddTokenAsync(httpClient);

            var response = await httpClient.PostAsync(
                $"{ApiConfig.BaseUrl}/api/Commandes/global/refresh",
                null
            );

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert(
                    "Succès",
                    "Commande globale envoyée à tous les postes.",
                    "OK"
                );

                await LoadDataFromApiAsync();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();

                await DisplayAlert(
                    "Erreur commande globale",
                    $"Code : {response.StatusCode}\nDétail : {error}",
                    "OK"
                );
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", ex.Message, "OK");
        }
    }
}