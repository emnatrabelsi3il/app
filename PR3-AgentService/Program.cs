using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PR3_AgentService;

Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "PR3 Agent Parc Informatique";
    })
    .ConfigureServices((context, services) =>
    {
        services.AddHttpClient("ApiClient")
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                return new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                    {
                        // Accepter le certificat HTTPS local uniquement pour localhost
                        return message.RequestUri != null &&
                               message.RequestUri.Host == "localhost";
                    }
                };
            });

        services.AddHostedService<Worker>();
    })
    .Build()
    .Run();