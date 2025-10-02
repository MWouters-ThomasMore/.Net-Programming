namespace WebAPIDemo.DTO.Orderlijn
{
    public class UpdateOrderLijnDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public double Aantal { get; set; }

        public int BestellingId { get; set; }

        public int ProductId { get; set; }
    }
}