using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using static PR3_Blazor.Components.Services.UtilisateurService;

namespace PR3_Blazor.Components.Services
{
    public class AuthService
{
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
        _httpContextAccessor = httpContextAccessor;


        }

        public string GetTokenFromSessionAsync()
        {
            var principal = _httpContextAccessor.HttpContext?.User;
            if (principal != null)
            {
                var tokenClaim = principal.FindFirst("token");
                if (tokenClaim != null)
                {
                    return tokenClaim.Value;
                }
            }
            return null;
        }
    }
}
