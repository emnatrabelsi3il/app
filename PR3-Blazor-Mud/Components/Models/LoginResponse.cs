using PR3_Blazor_Mud.Components.Models;

namespace PR3_Blazor_Mud.Components.Models
{
    public class LoginResponse
{
        public string Token { get; set; }
        public Utilisateur Utilisateur { get; set; }
    }
}
