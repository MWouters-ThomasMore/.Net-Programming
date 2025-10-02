namespace WebAPIDemo.Data.Repositories
{
    public class ProductRepo : GenericRepo<Product>, IProductRepo
    {
        public ProductRepo(WebAPIDemoContext context) : base(context)
        {
        }

        public async Task<List<Product>> SearchByNameAsync(string name)
        {
            return await _context.Producten
                 .Where(x => x.Naam.Contains(name))
                 .OrderBy(x => x.Naam)
                 .ToListAsync();
        }
    }
}