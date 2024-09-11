using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesWebApi.Services;

namespace MoviesWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiUsers : ControllerBase
    {
        private readonly IUser _user;

        public ApiUsers(IUser user)
        {
            _user = user;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]Rejester model)
        {
          var result =  await _user.RejsterAsunc(model);

            if (!result.ISAuthenticate)
                return BadRequest(result.Massege);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn([FromBody] Login model)
        {
            var result = await _user.LoginAsunc(model);

            if (!result.ISAuthenticate)
                return BadRequest(result.Massege);

            return Ok(result);
        }

        [HttpPost("roleofuser")]
        public async Task<IActionResult> UserRole([FromBody] AddRole model)
        {
            var result = await _user.AddRoleAsync(model);

            if(string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }

        [HttpPost("addrole")]
        public async Task<IActionResult> AddRole([FromBody]Role model)
        {
            var result = await _user.RoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }
    }
}
