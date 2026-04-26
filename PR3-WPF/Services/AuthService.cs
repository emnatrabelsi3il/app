using System;

using System.Threading.Tasks;
using System.IO;
using System.Net.Http;




namespace PR3_WPF.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        private static readonly string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AuthService", "token.txt");

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;

        }
        public async Task<string> GetToken()
        {
            var response = await _httpClient.PostAsync("https://localhost:7011/Auth", null);
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public void StoreToken(string data)
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                File.WriteAllText(filePath, data);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as necessary
                Console.WriteLine($"Error storing token: {ex.Message}");
            }
        }

        public string ReadToken()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return null;
                }

                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as necessary
                Console.WriteLine($"Error reading token: {ex.Message}");
                return null;
            }
        }


        public class JwtResponse
        {
            public string Token { get; set; }
        }
    }


}
