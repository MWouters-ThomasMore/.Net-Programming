namespace WebAPIDemo.Services
{
    public class PrijsService
    {
        public decimal BerekenTotaalPrijs(decimal prijs, decimal btw)
        {
            decimal prijsMetBtw = prijs + prijs * btw;
            if (prijs > 1000)
            {
                return Math.Round(prijsMetBtw * 0.9m, 2);
            }

            return Math.Round(prijsMetBtw, 2);
        }
    }
}
