using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Storage;

namespace MauiApp1.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        public AuthService(HttpClient httpClient)
        {

            _httpClient = httpClient;

        }


        
        public async Task<string> RetrieveTokenAsync()
        {
            try
            {
                return await SecureStorage.GetAsync("access_token");
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                Console.WriteLine($"Error retrieving token: {ex.Message}");
                return null;
            }
        }


    }
}
