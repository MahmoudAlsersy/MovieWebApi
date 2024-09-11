using Microsoft.EntityFrameworkCore;

namespace services
{
    public interface IGenres
    {
        public Task<IEnumerable<Genre>> GetAll();
        public Task<Genre> GetById(int id);
        public Task<Genre> Add(Genre genre);
        public Task<Genre> Update(Genre genre);
        public Task<Genre> Delete(int id);
        public Task<bool> GetGenreId(byte id);
    }

    public class ClsGenres : IGenres
    {
        private readonly ApplicationDbContext _context;

        public ClsGenres(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Genre> Add(Genre genre)
        {
           await  _context.AddAsync(genre);
            _context.SaveChanges();
            return genre;
        }

        public async Task<Genre> Delete(int id)
        {
            var data = await GetById(id);
            _context.Remove(data);
            _context.SaveChanges();
            return data;
        }

        public async Task<IEnumerable< Genre>> GetAll()
        {
            return await _context.Genres.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<Genre> GetById(int id)
        {
            return await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Genre> Update(Genre genre)
        {
            _context.Update(genre);
          await _context.SaveChangesAsync();
            return genre;
        }

        public async Task<bool> GetGenreId(byte id)
        {
            return await _context.Genres.AnyAsync(g => g.Id == id);
        }
    }
}
