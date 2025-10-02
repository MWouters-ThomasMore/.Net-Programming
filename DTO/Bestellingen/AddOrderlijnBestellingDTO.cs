namespace WebAPIDemo.DTO.Bestellingen
{
    public class AddOrderlijnBestellingDTO
    {
            [Required]
            public double Aantal { get; set; }

            public int ProductId { get; set; }
    }
}
