
namespace WebAPIDemo.Data.Repositories
{
    public interface IProductRepo: IRepo<Product>
    {
        Task<List<Product>> SearchByNameAsync(string name);
    }
}