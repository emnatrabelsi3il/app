using System;

using System.Threading.Tasks;
using System.IO;
using System.Net.Http;

using System.IdentityModel.Tokens.Jwt;
using System.Linq;


namespace PR3_WPF.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        private static readonly string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AuthService", "token.txt");
        private static readonly string rolePath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
    "AuthService",
    "role.txt"
);

        public void StoreRole(string role)
        {
            string directoryPath = Path.GetDirectoryName(rolePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(rolePath, role);
        }

        public string ReadRole()
        {
            if (!File.Exists(rolePath))
            {
                return "user";
            }

            return File.ReadAllText(rolePath);
        }
        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Méthode pour récupérer le rôle depuis le token JWT
        public string GetUserRole()
        {
            return ReadRole();
        }

        public async Task<string> GetToken()
        {
            var response = await _httpClient.PostAsync("https://localhost:7011/Auth", null);
            response.EnsureSuccessStatusCode();

            string data = await response.Content.ReadAsStringAsync();
            return data;
        }
        public void ClearToken()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
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
                Console.WriteLine($"Error reading token: {ex.Message}");
                return null;
            }
        }
    }
}