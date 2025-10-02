namespace WebAPIDemo.DTO.Bestellingen
{
    public class AddBestellingDTO
    {
        [Required]
        public int KlantId { get; set; }

        public List<AddOrderlijnBestellingDTO> Orderlijnen { get; set; } = default!;
    }
}