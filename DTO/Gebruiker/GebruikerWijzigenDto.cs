namespace WebAPIDemo.DTO.Gebruiker
{
    public class GebruikerWijzigenDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string? Naam { get; set; }

        public DateTime? Geboortedatum { get; set; }

        public string UserName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }
    }
}