namespace MauiApp1;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;

using System.Net.Http.Headers;
using MauiApp1.Model;
using MauiApp1.Services;
using Newtonsoft.Json;
public partial class PostePage : ContentPage
{
    private AuthService authService;

    public ObservableCollection<Poste> postes { get; set; } = new ObservableCollection<Poste>();
    public PostePage()
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
            var jwtToken = await SecureStorage.GetAsync("access_token");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var poste = await httpClient.GetFromJsonAsync<List<Poste>>("http://10.0.2.2:5011/api/Postes");

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
                await DisplayAlert("Error", "No data received from server", "OK");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}