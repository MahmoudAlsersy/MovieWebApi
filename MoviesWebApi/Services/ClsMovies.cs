using Microsoft.EntityFrameworkCore;

namespace MoviesWebApi.Services
{
    public interface IMovies
    {
        public Task<Movie> GetById(int id);
        public Task<Movie> Delete(int id);
        public Task<IEnumerable<Movie>> GetAll();
        public Task<Movie> Add(Movie movie);
        public Movie Update(Movie movie);
    }
    public class ClsMovies : IMovies
    {
        private readonly ApplicationDbContext _context;

        public ClsMovies(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Movie> Add(Movie movie)
        {
           await _context.Movies.AddAsync(movie);
            _context.SaveChanges();
            return movie;
        }

        public Task<Movie> Delete(int id)
        {
            var data = GetById(id);
            _context.Remove(data);
            _context.SaveChanges();
            return data;
        }

        public async Task<IEnumerable<Movie>> GetAll()
        {
            return await _context.Movies.Include(m=>m.Genre)
                .OrderByDescending(m => m.Rate).ToListAsync();
        }

        public async Task<Movie> GetById(int id)
        {
            return await _context.Movies.Include(m=>m.Genre).FirstOrDefaultAsync(m => m.Id == id);
        }

        public Movie Update(Movie movie)
        {
            _context.Update(movie);
            _context.SaveChanges();
            return movie;
        }
    }
}
