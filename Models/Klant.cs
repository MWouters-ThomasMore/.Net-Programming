

namespace WebAPIDemo.Models
{
    public class Klant
    {
        public int Id { get; set; }
        [Required]
        public string Naam { get; set; } = default!;
        [Required]
        public string Voornaam { get; set; } = default!;
        [DataType(DataType.Date), Display(Name = "Datum Aangemaakt")]

        public DateTime AangemaaktDatum { get; set; }
    }
}
