
namespace WebAPIDemo.Services
{
    public class BTWService
    {
        public double bedrag;
        public double btw;
        public double resultaat;

        public void BerekenBTW()
        {
            resultaat = bedrag + (bedrag * btw);

        }

        public double BerekenBTW(double bedrag, double btwPercentage)
        {
            return (bedrag + (bedrag * btwPercentage));
        }
    }
}