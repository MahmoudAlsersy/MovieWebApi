using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesWebApi.Models;
using MoviesWebApi.Services;
using services;

namespace MoviesWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovies _movie;
        private readonly IGenres _genre;
        private readonly IMapper _Mapper;
        private new List<string> Allow = new List<string> { ".jpg", ".png" };
        private long MaxLenth = 1048576;

        public MoviesController(IMovies movie, IGenres genre, IMapper mapper)
        {
            _movie = movie;
            _genre = genre;
            _Mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var data = await _movie.GetAll();
            var movie = _Mapper.Map<IEnumerable<MoviesGenreDetails>>(data);
            return Ok(movie);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _movie.GetById(id);

            if (data == null)
                return NotFound();

            var movie = _Mapper.Map<MoviesGenreDetails>(data);

            return Ok(movie);

        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            if(dto.Poster == null) 
            {
                return BadRequest("Poster is Required");
            }

            if (!Allow.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .jpg and .png images are Alloewd");

            if (dto.Poster.Length > MaxLenth)
                return BadRequest("Sholde be size less than 1MB");

            bool isvalid = await _genre.GetGenreId(dto.GenreId);

            if (!isvalid)
                return BadRequest("InValid Id");

            using var datastrem = new MemoryStream();
            dto.Poster.CopyTo(datastrem);

            var data = _Mapper.Map<Movie>(dto);
            data.Poster = datastrem.ToArray();

            await _movie.Add(data);
            return Ok(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _movie.GetById(id);

            if (data == null)
                return BadRequest($"No Movie With ID : {id}");

            await _movie.Delete(id);

            return Ok(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id ,[FromForm]MovieDto dto)
        {

            var data = await _movie.GetById(id);

            if(data == null)
                 return NotFound($"Not Movie Was Found With Id:{id}");

            bool isvalid = await _genre.GetGenreId(dto.GenreId);
            if (!isvalid)
                return BadRequest("InValid Id");

            if(dto.Poster!= null)
            {
                if (!Allow.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .jpg and .png images are Alloewd");

                if (dto.Poster.Length > MaxLenth)
                    return BadRequest("Sholde be size less than 1MB");

                using var datastrem = new MemoryStream();
                dto.Poster.CopyTo(datastrem);
                data.Poster = datastrem.ToArray();
            }

            data.Title = dto.Title;
            data.Rate = dto.Rate;
            data.Year = dto.Year;
            data.Dscription = dto.Dscription;
            data.GenreId = dto.GenreId;
            

             _movie.Update(data);
            return Ok(data);
        }
    }
}
      