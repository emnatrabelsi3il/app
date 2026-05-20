namespace MauiApp1;

using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MauiApp1.Model;

public partial class EtablissementPage : ContentPage
{
	public ObservableCollection<Etablissement> etablissements { get; set; } = new ObservableCollection<Etablissement>();

	private bool _alreadyLoaded = false;

	public EtablissementPage()
	{
		InitializeComponent();
		BindingContext = this;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (_alreadyLoaded)
			return;

		await LoadDataFromApiAsync();
	}

	private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
	{
	}

	private async Task<string?> WaitForTokenAsync()
	{
		for (int i = 0; i < 10; i++)
		{
			var token = await SecureStorage.GetAsync("access_token");

			if (!string.IsNullOrWhiteSpace(token))
				return token;

			await Task.Delay(500);
		}

		return null;
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

			var jwtToken = await WaitForTokenAsync();

			if (string.IsNullOrWhiteSpace(jwtToken))
			{
				await DisplayAlert(
					"Erreur",
					"Token introuvable après attente. Réessayez dans quelques secondes.",
					"OK"
				);

				_alreadyLoaded = false;
				return;
			}

			httpClient.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("Bearer", jwtToken);

			var response = await httpClient.GetAsync(
				$"{ApiConfig.BaseUrl}/api/Etablissements"
			);

			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				await DisplayAlert(
					"Erreur authentification",
					"L'API a refusé l'accès. Le token est absent, expiré ou invalide.",
					"OK"
				);

				_alreadyLoaded = false;
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

				_alreadyLoaded = false;
				return;
			}

			var etablissement = await response.Content.ReadFromJsonAsync<List<Etablissement>>();

			if (etablissement != null)
			{
				etablissements.Clear();

				foreach (var parc in etablissement)
				{
					etablissements.Add(parc);
				}

				MyListView.ItemsSource = etablissements;

				_alreadyLoaded = true;
			}
			else
			{
				await DisplayAlert("Erreur", "Aucune donnée reçue depuis le serveur.", "OK");
				_alreadyLoaded = false;
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Erreur EtablissementPage : {ex.Message}");
			await DisplayAlert("Erreur chargement", ex.Message, "OK");

			_alreadyLoaded = false;
		}
	}
}