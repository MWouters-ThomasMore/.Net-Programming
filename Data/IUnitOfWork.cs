using WebAPIDemo.Data.Repositories;

namespace WebAPIDemo.Data
{
    public interface IUnitOfWork
    {
        IBestellingRepo BestellingRepo { get; }
        IKlantRepo KlantRepo { get; }
        IRepo<Orderlijn> OrderlijnRepo { get; }
        IProductRepo ProductRepo { get; }
        Task SaveChangesAsync();
    }
}