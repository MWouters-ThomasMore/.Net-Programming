namespace WebAPIDemo.DTO.Gebruiker
{
    public class RegistratieDTO
    {
        [Required(ErrorMessage = "Naam is verplicht")]
        [StringLength(100)]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "E-mail is niet goed gevormd")]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Paswoorden komen niet overeen!")]
        public string ConfirmPassword { get; set; }

        public string PhoneNumber { get; set; }
    }
}
