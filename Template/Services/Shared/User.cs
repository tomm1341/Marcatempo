using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Template.Services.Shared
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Cognome { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [RuoloValido(new[] { "ResponsabileEsterno", "ResponsabileInterno", "Utente" })]
        public string Ruolo { get; set; }

        /// <summary>
        /// Checks if password passed as parameter matches with the Password of the current user
        /// </summary>
        /// <param name="password">password to check</param>
        /// <returns>True if passwords match. False otherwise.</returns>
        public bool IsMatchWithPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;

            var sha256 = SHA256.Create();
            var testPassword = System.Convert.ToBase64String(sha256.ComputeHash(Encoding.ASCII.GetBytes(password)));

            return this.Password == testPassword;
        }
    }

    public class RuoloValidoAttribute : ValidationAttribute
    {
        private readonly string[] _valoriPermessi;

        public RuoloValidoAttribute(string[] valoriPermessi)
        {
            _valoriPermessi = valoriPermessi;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || !_valoriPermessi.Contains(value.ToString()))
            {
                return new ValidationResult($"Valore non valido. I valori permessi sono: {string.Join(", ", _valoriPermessi)}");
            }
            return ValidationResult.Success;
        }
    }
}
