using System.ComponentModel.DataAnnotations;

namespace PR3_Blazor.Components.Models.ViewModels
{
    public class LoginViewModel
{
        [Required(AllowEmptyStrings =false, ErrorMessage = "Renseigner un nom d'utilisateur")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Renseigner un mot de passe")]
        public string Password { get; set; }

    }
}
