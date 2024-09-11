using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using services;

namespace MoviesWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenres  _genre;

        public GenresController(IGenres genre)
        {
            _genre = genre;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _genre.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenerDto dto)
        {
            Genre genre = new Genre()
            {
                Name = dto.name
            };

           await _genre.Add(genre);

            return Ok(genre);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditAsync(int id ,[FromBody] GenerDto dto)
        {
            var genre = await _genre.GetById(id);

            if(genre == null)
                return NotFound($"Not Genre Was Found With Id:{id}");

            genre.Name = dto.name;
           
           await  _genre.Update(genre);

            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var genre =await _genre.GetById(id);

            if (genre == null)
                return NotFound($"Not Genre Was Found With Id:{id}");

           await _genre.Delete(id);

            return Ok(genre);
        }
    }
}
