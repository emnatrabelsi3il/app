namespace MauiApp1;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;

using System.Net.Http.Headers;
using MauiApp1.Model;
using MauiApp1.Services;
using Newtonsoft.Json;

public partial class EtablissementPage : ContentPage
{

    public ObservableCollection<Etablissement> etablissements { get; set; } = new ObservableCollection<Etablissement>();

    public EtablissementPage()
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
            //authService = new AuthService(httpClient);
            var jwtToken = await SecureStorage.GetAsync("access_token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var etablissement = await httpClient.GetFromJsonAsync<List<Etablissement>>("http://10.0.2.2:5011/api/Etablissements");

                if (etablissement != null)
                {
                    etablissements.Clear();

                    foreach (var parc in etablissement)
                    {
                        etablissements.Add(parc);
                    }

                    MyListView.ItemsSource = etablissements;
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