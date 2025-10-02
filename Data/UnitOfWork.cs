using WebAPIDemo.Data.Repositories;

namespace WebAPIDemo.Data
{
    // Unit Of Work bevat alle repositories en een manier om op te slaan.
    public class UnitOfWork : IUnitOfWork
    {
        private WebAPIDemoContext _context;
        private IBestellingRepo bestellingRepo;
        private IKlantRepo klantRepo;
        private IRepo<Orderlijn> orderlijnRepo;
        private IProductRepo productRepo;

        public UnitOfWork(WebAPIDemoContext context)
        {
            _context = context;
        }

        public IBestellingRepo BestellingRepo
        {
            get
            {
                if (bestellingRepo == null)
                {
                    bestellingRepo = new BestellingRepo(_context);
                }

                return bestellingRepo;
            }
        }

        public IKlantRepo KlantRepo
        {
            get
            {
                if (klantRepo == null)
                {
                    klantRepo = new KlantRepo(_context);
                }

                return klantRepo;
            }
        }

        public IRepo<Orderlijn> OrderlijnRepo
        {
            get
            {
                if (orderlijnRepo == null)
                {
                    orderlijnRepo = new GenericRepo<Orderlijn>(_context);
                }

                return orderlijnRepo;
            }
        }

        public IProductRepo ProductRepo
        {
            get
            {
                if (productRepo == null)
                {
                    productRepo = new ProductRepo(_context);
                }

                return productRepo;
            }
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}