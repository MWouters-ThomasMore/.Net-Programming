namespace WebAPIDemo.DTO.Bestellingen
{
    public class KlantBestellingenDTO
    {
        public string KlantNaam { get; set; }

        // EXAMEN TIP: NOOIT modellen in DTO's!
        public List<BestellingDTO> Bestellingen{ get; set; }
    }
}
