using System.ComponentModel.DataAnnotations;

namespace ProductManagementSystem.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Benutzername/E-Mail ist erforderlich")]
        public string LoginCredential { get; set; }

        [Required(ErrorMessage = "Passwort ist erforderlich")]
        public string Passwort { get; set; }
    }


}
