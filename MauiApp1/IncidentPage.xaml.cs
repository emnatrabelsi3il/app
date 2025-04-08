namespace MauiApp1;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;

using System.Net.Http.Headers;
using MauiApp1.Model;
using MauiApp1.Services;
using Newtonsoft.Json;
public partial class IncidentPage : ContentPage
{

    private AuthService authService;

    public ObservableCollection<Incident> incidents { get; set; } = new ObservableCollection<Incident>();
    public IncidentPage()
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
            /*var jwtToken = await SecureStorage.GetAsync("access_token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);*/

            var incident = await httpClient.GetFromJsonAsync<List<Incident>>("http://10.0.2.2:5011/api/Incidents");

            if (incident != null)
            {
                incidents.Clear();

                foreach (var inc in incident)
                {
                    incidents.Add(inc);
                }

                MyListView.ItemsSource = incidents;
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