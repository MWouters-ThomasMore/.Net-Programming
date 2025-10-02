namespace WebAPIDemo.Models
{
    public class Klant : IModel
    {
        public int Id { get; set; }

        [Required]
        public string Naam { get; set; } = default!;

        [Required]
        public string Voornaam { get; set; } = default!;

        [DataType(DataType.Date), Display(Name = "Datum Aangemaakt")]
        public DateTime AangemaaktDatum { get; set; }

        // Navigation Properties
        public List<Bestelling> Bestellingen { get; set; } = default!;
    }
}