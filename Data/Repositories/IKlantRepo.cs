namespace WebAPIDemo.Data.Repositories
{
    public interface IKlantRepo : IRepo<Klant>
    {
        Task<List<Klant>> GetKlantenMetBestellingen();
    }
}