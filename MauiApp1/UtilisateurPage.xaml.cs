namespace MauiApp1;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;

using System.Net.Http.Headers;
using MauiApp1.Model;
using MauiApp1.Services;
using Newtonsoft.Json;
public partial class UtilisateurPage : ContentPage
{
    private AuthService authService;

    public ObservableCollection<Utilisateur> utilisateurs { get; set; } = new ObservableCollection<Utilisateur>();

    public UtilisateurPage()
    {
        InitializeComponent();
        BindingContext = this;

        LoadDataFromApi();
    }

    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
    }
    private async void LoadDataFromApi()
    {
        var httpClient = new HttpClient();
        try
        {
            authService = new AuthService(httpClient);
            var jwtToken = await authService.RetrieveTokenAsync();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var utilisateur = await httpClient.GetFromJsonAsync<List<Utilisateur>>("http://10.0.2.2:5011/api/Utilisateurs");

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
                await DisplayAlert("Error", "No data received from server", "OK");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}