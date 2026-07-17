
using Domain.Database;
using Domain.Database.Context;
using Domain.Interfaces.UsersManagement;
using Domain.ViewModels.UsersManagement;
using Microsoft.AspNetCore.Mvc;

namespace FavoritePlacesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly FavoritePlacesContext _context;
        private readonly IUsersService _users;

        public UsersController(FavoritePlacesContext context, IUsersService users)
        {
            _context = context;
            _users = users;
        }

        //[HttpGet]
        //public ActionResult<IEnumerable<Users>> Get()
        //{
        //    var users = _context.Users.ToList();

        //    return Ok(new { users = users });
        //}

        [HttpGet("GetUsers")]
        public ActionResult<List<Users>> GetUsers()
        {
            var users = _users.GetUsers();

            return Ok(new GetUsersViewModel { users = users });
        }

        [HttpPost("login")]
        public async Task<IActionResult> PostUserPlacesAsync([FromBody] UserLoginViewModel request)
        {
            try
            {
                var LoggedinUser = await _users.LoginUser(request);

                return Ok(new { currentUser = LoggedinUser });
            } catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> PostUserAsync([FromBody] UserLoginViewModel request)
        {
            try
            {
                var registeredUser = await _users.RegisterUser(request);

                return Ok(new { currentUser = registeredUser });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
