namespace WebAPIDemo.Models
{
    public class Product : IModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Naam { get; set; }

        [StringLength(500)]
        public string? Beschrijving { get; set; }

        [Required]
        public decimal Prijs { get; set; }

        // Navigation Properties
        public List<Orderlijn> Orderlijnen { get; set; } = default!;
    }
}