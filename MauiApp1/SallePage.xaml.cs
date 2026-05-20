namespace MauiApp1;

using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MauiApp1.Model;

public partial class SallePage : ContentPage
{
    public ObservableCollection<SalleAffichage> salles { get; set; } = new ObservableCollection<SalleAffichage>();

    private bool _alreadyLoaded = false;

    public SallePage()
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

            var responseSalles = await httpClient.GetAsync(
                "https://hqx6q9wt-7011.uks1.devtunnels.ms/api/Salles"
            );

            if (responseSalles.StatusCode == HttpStatusCode.Unauthorized)
            {
                await DisplayAlert(
                    "Erreur authentification",
                    "L'API a refusé l'accès aux salles. Le token est absent, expiré ou invalide.",
                    "OK"
                );
                return;
            }

            if (!responseSalles.IsSuccessStatusCode)
            {
                var errorContent = await responseSalles.Content.ReadAsStringAsync();

                await DisplayAlert(
                    "Erreur API Salles",
                    $"Code : {responseSalles.StatusCode}\nDétail : {errorContent}",
                    "OK"
                );
                return;
            }

            var responseEtablissements = await httpClient.GetAsync(
                "https://hqx6q9wt-7011.uks1.devtunnels.ms/api/Etablissements"
            );

            if (responseEtablissements.StatusCode == HttpStatusCode.Unauthorized)
            {
                await DisplayAlert(
                    "Erreur authentification",
                    "L'API a refusé l'accès aux établissements. Le token est absent, expiré ou invalide.",
                    "OK"
                );
                return;
            }

            if (!responseEtablissements.IsSuccessStatusCode)
            {
                var errorContent = await responseEtablissements.Content.ReadAsStringAsync();

                await DisplayAlert(
                    "Erreur API Établissements",
                    $"Code : {responseEtablissements.StatusCode}\nDétail : {errorContent}",
                    "OK"
                );
                return;
            }

            var listeSalles = await responseSalles.Content.ReadFromJsonAsync<List<Salle>>();
            var listeEtablissements = await responseEtablissements.Content.ReadFromJsonAsync<List<Etablissement>>();

            if (listeSalles == null)
            {
                await DisplayAlert("Erreur", "Aucune salle reçue depuis le serveur.", "OK");
                return;
            }

            if (listeEtablissements == null)
            {
                await DisplayAlert("Erreur", "Aucun établissement reçu depuis le serveur.", "OK");
                return;
            }

            salles.Clear();

            foreach (var salle in listeSalles)
            {
                var etablissement = listeEtablissements
                    .FirstOrDefault(e => e.Id == salle.EtablissementId);

                salles.Add(new SalleAffichage
                {
                    Id = salle.Id,
                    Numero = salle.Numero,
                    EtablissementId = salle.EtablissementId,
                    NomEtablissement = etablissement != null
          ? etablissement.Nom
          : "Établissement inconnu"
                });
            }

            MyListView.ItemsSource = salles;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur SallePage : {ex.Message}");
            await DisplayAlert("Erreur chargement", ex.Message, "OK");
        }
    }

    
    public class SalleAffichage
    {
        public long Id { get; set; }
        public string? Numero { get; set; }
        public long EtablissementId { get; set; }
        public string? NomEtablissement { get; set; }
    }
}