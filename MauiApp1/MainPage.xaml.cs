using MauiApp1.Model;
using MauiApp1.Services;

using MauiApp1.Services;
using System.Text.Json;
using System.Net.Http.Json;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            GetToken();

        }
        public async void GetToken()
        {


               var loginRequest = new LoginRequest
               {
                   Login = "admin",
                   MotDePasse = "admin"
               };

               var result = await LoginAsync(loginRequest);
               var token = result.token;
            try
            {
                await SecureStorage.Default.SetAsync("access_token", token);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }

        }

        public async Task<(bool isSuccess, string token, Utilisateur utilisateur)> LoginAsync(LoginRequest loginRequest)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync("http://10.0.2.2:5011/api/utilisateurs/login", loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response JSON: {jsonString}");

                    var result = System.Text.Json.JsonSerializer.Deserialize<LoginResponse>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (result != null)
                    {
                        return (true, result.Token, result.Utilisateur);
                    }
                }

                return (false, null, null);
            }
        }

    }

}
