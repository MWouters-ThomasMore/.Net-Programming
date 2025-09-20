

namespace WebAPIDemo.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="De naam moet ingevuld worden.")]
        [MinLength(3, ErrorMessage ="De naam moet minstens 3 tekens bevatten.")]
        [StringLength(100, ErrorMessage ="De naam mag niet meer dan 100 tekens bevatten.")]
        public string Naam { get; set; }

        [StringLength(500, ErrorMessage = "De beschrijving mag niet langer zijn dan 500 tekens.")]
        public string? Beschrijving { get; set; }

        [Required(ErrorMessage = "De prijs van het product is verplicht.")]
        [Range(0.01, 10000.00, ErrorMessage = "De prijs moet tussen 0,01 en 10.000 liggen.")]
        public decimal Prijs { get; set; }

    }
}
