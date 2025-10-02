namespace WebAPIDemo.Models
{
    public class Orderlijn : IModel
    {
        public int Id { get; set; }

        [Required]
        public double Aantal { get; set; }

        // Navigation Properties
        public int BestellingId { get; set; }

        public Bestelling Bestelling { get; set; } = default!;

        public int ProductId { get; set; }

        public Product Product { get; set; } = default!;
    }
}