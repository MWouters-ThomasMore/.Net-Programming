using WebAPIDemo.DTO.Orderlijn;

namespace WebAPIDemo.DTO.Bestellingen
{
    public class UpdateBestellingDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int KlantId { get; set; }

        public List<UpdateOrderLijnDTO> Orderlijnen { get; set; } = default!;
    }
}
