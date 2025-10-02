namespace WebAPIDemo.DTO.Klant
{
    public class AddKlantDTO
    {
        [Required]
        public string Naam { get; set; } = default!;

        [Required]
        public string Voornaam { get; set; } = default!;
    }
}
