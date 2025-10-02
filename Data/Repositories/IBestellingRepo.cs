
namespace WebAPIDemo.Data.Repositories
{
    public interface IBestellingRepo: IRepo<Bestelling>
    {
        Task<List<Bestelling>> GetAllIncludingProducts();

        Task<List<Bestelling>> GetBestellingenMetAlleInfo();

        Task<Bestelling> GetBestellingIncludingProducts(int id);
    }
}