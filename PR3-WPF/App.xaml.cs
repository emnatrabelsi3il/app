using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;

namespace PR3_WPF
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string disconnedAPI = "http://localhost:5011/api/Postes/DisconnectByMacAdress";
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Code to run when the application starts
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            string macAdress = GetMacAddress();
            using (HttpClient client = new HttpClient())
            {
                var response = await client.PutAsync($"{disconnedAPI}/{macAdress}", null);
                if (response.IsSuccessStatusCode)
                {
                    // Successfully disconnected
                }
                else
                {
                    // Handle error response
                }
            }
        }


        private string GetMacAddress()
        {
            var nic = NetworkInterface.GetAllNetworkInterfaces()
                                      .FirstOrDefault(n => n.OperationalStatus == OperationalStatus.Up);
            return nic?.GetPhysicalAddress().ToString() ?? "00:00:00:00:00:00";
        }
    }

}
