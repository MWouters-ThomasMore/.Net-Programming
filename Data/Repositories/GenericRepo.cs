namespace WebAPIDemo.Data.Repositories
{
    // Generic Repository: Vangt alle herhalende Get, Update code op in 1 gedeelde codebase
    public class GenericRepo<T> : IRepo<T> where T : class, IModel
    {
        protected WebAPIDemoContext _context;

        public GenericRepo(WebAPIDemoContext context)
        {
            _context = context;
        }

        public async Task Add(T item)
        {
            await _context.Set<T>().AddAsync(item);
        }

        public async Task Update(T item)
        {
            _context.Set<T>().Update(item);
        }

        public async Task Delete(T item)
        {
            _context.Set<T>().Remove(item);
        }

        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetObject(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public bool ItemExists(int id)
        {
            return _context.Set<T>().Any(e => e.Id == id);
        }
    }
}