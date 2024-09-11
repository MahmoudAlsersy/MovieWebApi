using AutoMapper;

namespace MoviesWebApi.Helper
{
    public class Mapiing :Profile
    {
        public Mapiing()
        {
            CreateMap<Movie, MoviesGenreDetails>();
            CreateMap<MovieDto, Movie>()
                .ForMember(Map => Map.Poster, scr => scr.Ignore());
        }
    }
}
