using System.Net;
using System.Net.Http.Json;

namespace PR3_AgentService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        private const string ApiBaseUrl = "https://localhost:7011";
        private const string HeartbeatUrl = ApiBaseUrl + "/api/Postes/heartbeat";
        private const string LogFile = @"C:\PR3Agent\agent-log.txt";

        public Worker(ILogger<Worker> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            WriteLog("Service démarré.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var info = AgentInfoCollector.Collect();

                    var client = _httpClientFactory.CreateClient("ApiClient");

                    client.DefaultRequestVersion = HttpVersion.Version11;
                    client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;

                    await SendHeartbeat(client, info, stoppingToken);

                    await ExecutePendingCommands(client, info, stoppingToken);
                }
                catch (Exception ex)
                {
                    WriteLog($"EXCEPTION : {ex.Message}");
                    WriteLog(ex.ToString());
                }

                await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
            }
        }

        private static async Task SendHeartbeat(HttpClient client, AgentHeartbeatDto info, CancellationToken stoppingToken)
        {
            WriteLog($"Envoi heartbeat vers : {HeartbeatUrl}");
            WriteLog($"Machine : {info.NomMachine} | MAC : {info.MacAdress}");

            var response = await client.PostAsJsonAsync(
                HeartbeatUrl,
                info,
                stoppingToken
            );

            var responseContent = await response.Content.ReadAsStringAsync(stoppingToken);

            if (response.IsSuccessStatusCode)
            {
                WriteLog($"SUCCESS : Heartbeat envoyé. Réponse API : {responseContent}");
            }
            else
            {
                WriteLog($"ERREUR API HEARTBEAT : {response.StatusCode} | {responseContent}");
            }
        }

        private static async Task ExecutePendingCommands(HttpClient client, AgentHeartbeatDto info, CancellationToken stoppingToken)
        {
            var macEncoded = Uri.EscapeDataString(info.MacAdress);

            var pendingUrl = $"{ApiBaseUrl}/api/Commandes/pending/{macEncoded}";

            WriteLog($"Recherche commandes en attente : {pendingUrl}");

            var commandes = await client.GetFromJsonAsync<List<AgentCommandDto>>(
                pendingUrl,
                stoppingToken
            );

            if (commandes == null || commandes.Count == 0)
            {
                WriteLog("Aucune commande en attente.");
                return;
            }

            foreach (var commande in commandes)
            {
                WriteLog($"Commande reçue : ID={commande.Id}, Type={commande.TypeCommande}, Portée={commande.Portee}");

                bool success = false;
                string result = "";

                try
                {
                    if (commande.TypeCommande == "RefreshInfo")
                    {
                        var refreshedInfo = AgentInfoCollector.Collect();

                        await SendHeartbeat(client, refreshedInfo, stoppingToken);

                        success = true;
                        result = "Informations du poste actualisées avec succès.";
                    }
                    else
                    {
                        success = false;
                        result = $"Type de commande inconnu : {commande.TypeCommande}";
                    }
                }
                catch (Exception ex)
                {
                    success = false;
                    result = "Erreur pendant l'exécution : " + ex.Message;
                }

                var completeDto = new CompleteCommandDto
                {
                    Success = success,
                    Resultat = result
                };

                var completeUrl = $"{ApiBaseUrl}/api/Commandes/{commande.Id}/complete";

                var completeResponse = await client.PostAsJsonAsync(
                    completeUrl,
                    completeDto,
                    stoppingToken
                );

                var completeContent = await completeResponse.Content.ReadAsStringAsync(stoppingToken);

                WriteLog($"Résultat commande {commande.Id} : {completeResponse.StatusCode} | {completeContent}");
            }
        }

        private static void WriteLog(string message)
        {
            try
            {
                File.AppendAllText(
                    LogFile,
                    $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}"
                );
            }
            catch
            {
                // Ignorer pour ne pas bloquer le service
            }
        }
    }
}