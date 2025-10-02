
namespace WebAPIDemo.Data.Repositories
{
    public interface IRepo<T>
    {
        Task Add(T item);

        Task Update(T item);

        Task Delete(T item);

        Task<T> GetObject(int id);

        Task<List<T>> GetAll();

        bool ItemExists(int id);
    }
}