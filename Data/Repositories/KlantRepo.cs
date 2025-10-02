namespace WebAPIDemo.Data.Repositories
{
    public class KlantRepo : GenericRepo<Klant>, IKlantRepo
    {
        public KlantRepo(WebAPIDemoContext context) : base(context)
        {
        }

        public async Task<List<Klant>> GetKlantenMetBestellingen()
        {
            return await _context.Klanten
                            .Include(x => x.Bestellingen)
                            .ThenInclude(x => x.Orderlijnen)
                            .ThenInclude(x => x.Product)
                            .ToListAsync();
        }
    }
}