namespace WebAPIDemo.Services
{
    public class PrijsService
    {
        private readonly PrijsRepo _prijsRepo;

        public PrijsService(PrijsRepo prijsRepo)
        {
            _prijsRepo = prijsRepo;
        }

        public decimal BerekenTotaalPrijs(int id, decimal btwPercentage)
        {
            decimal prijs = _prijsRepo.GetPrijsVoorProduct(id);

            if (prijs < 0)
            {
                throw new ArgumentException("Prijs moet groter zijn dan of gelijk aan 0.", nameof(prijs));
            }

            //bereken eerst de btw
            decimal prijsMetBtw = prijs + (prijs * btwPercentage);

            if (prijs > 1000)
            {
                //geef 10% korting
                return Math.Round(prijsMetBtw * 0.9m, 2);
            }

            return Math.Round(prijsMetBtw, 2);

        }


    }
}
