namespace MauiApp1;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;

using System.Net.Http.Headers;
using MauiApp1.Model;
using MauiApp1.Services;
using Newtonsoft.Json;

public partial class SallePage : ContentPage
{
    private AuthService authService;

    public ObservableCollection<Salle> salles { get; set; } = new ObservableCollection<Salle>();

    public SallePage()
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
            /*authService = new AuthService(httpClient);
            var jwtToken = await authService.RetrieveTokenAsync();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);*/

            var salle = await httpClient.GetFromJsonAsync<List<Salle>>("http://10.0.2.2:5011/api/Salles");

            if (salle != null)
            {
                salles.Clear();

                foreach (var sal in salle)
                {
                    salles.Add(sal);
                }

                MyListView.ItemsSource = salles;
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