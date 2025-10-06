namespace WebAPIDemo.DTO.Gebruiker
{
    public class LoginDTO
    {
        [EmailAddress(ErrorMessage = "Ongeldige email")]
        [Required(ErrorMessage = "Email is verplicht")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Paswoord is verplicht")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
