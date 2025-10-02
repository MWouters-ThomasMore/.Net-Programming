namespace WebAPIDemo.DTO.Orderlijn
{
    public class AddOrderLijnDTO
    {
        [Required]
        public double Aantal { get; set; }

        public int BestellingId { get; set; }

        public int ProductId { get; set; }
    }
}