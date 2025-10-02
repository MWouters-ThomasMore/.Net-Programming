namespace WebAPIDemo.Data.Repositories
{
    // Repo: communicatie met DB
    // Bezit minstens :GetAll, GetOne, Add, Update & Delete
    public class BestellingRepo : GenericRepo<Bestelling>, IBestellingRepo
    {
        public BestellingRepo(WebAPIDemoContext context) : base(context)
        {
        }

        public async Task<List<Bestelling>> GetBestellingenMetAlleInfo()
        {
            return await _context.Bestellingen
                .Include(x => x.Klant)
                .Include(x => x.Orderlijnen)
                .ThenInclude(x => x.Product)
                .ToListAsync();
        }

        public async Task<List<Bestelling>> GetAllIncludingProducts()
        {
            return await _context.Bestellingen
                .Include(x => x.Orderlijnen)
                .ThenInclude(x => x.Product)
                .ToListAsync();
        }

        public async Task<Bestelling> GetBestellingIncludingProducts(int id)
        {
            return await _context.Bestellingen
                .Include(x => x.Orderlijnen)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}