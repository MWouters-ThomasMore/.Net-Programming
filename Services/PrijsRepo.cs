namespace WebAPIDemo.Services
{
    public class PrijsRepo
    {

        public virtual decimal GetPrijsVoorProduct(int Id)
        {
            return new Random().Next(200, 2000);
        }

    }
}
