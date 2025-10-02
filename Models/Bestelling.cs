namespace WebAPIDemo.Models
{
    public class Bestelling : IModel
    {
        public int Id { get; set; }

        public int KlantId { get; set; }

        // Navigation Properties
        public Klant Klant { get; set; } = default!;

        public List<Orderlijn> Orderlijnen { get; set; } = default!;
    }
}