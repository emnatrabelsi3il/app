using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace PR3_Blazor_Mud.Components.Services
{
    public class AuthService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<string?> GetTokenAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                return user.FindFirst("token")?.Value;
            }

            return null;
        }
    }
}