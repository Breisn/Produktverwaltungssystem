using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagementSystem.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Kundennummer { get; set; }

        [Required(ErrorMessage = "Die E-Mail-Adresse ist erforderlich.")]
        [EmailAddress(ErrorMessage = "Die E-Mail-Adresse ist ungültig.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Der Benutzername ist erforderlich.")]
        [MinLength(5, ErrorMessage = "Der Benutzername muss mindestens 5 Zeichen lang sein.")]
        public string? Benutzername { get; set; }

        [Required(ErrorMessage = "Das Passwort ist erforderlich.")]
        public string? Passwort { get; set; }
    }
}
