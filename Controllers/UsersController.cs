
using System.Data.Entity;
using Domain.Database;
using Domain.Database.Context;
using Domain.Interfaces.UsersManagement;
using Domain.ViewModels.UsersManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        [HttpGet("user-places")]
        public ActionResult<IEnumerable<UserFavoritePlaces>> GetUserPlaces(int user_id)
        {
            var placeIds = _context.UserFavoritePlaces
                                   .Where(userPlace => userPlace.UserId == user_id) // หาที่ user_id เหมือนกัน
                                   .Select(userPlace => userPlace.PlaceId) //เลือก place_id ทั้งหมดที่ user_id เหมือนกัน
                                   .ToArray();

            var userPlaces = _context.Places
                                         .Where(p => placeIds.Contains(p.PlaceId)) //เลือก place ที่ place_id เหมือนกัน
                                         .ToList();

            return Ok(new { places = userPlaces });
        }

        [HttpPost("login")]
        public async Task<IActionResult> PostUserPlacesAsync([FromBody] Dictionary<string, string> data)
        {
            // ดึงค่าออกมาจาก Dictionary โดยระบุ Key ให้ตรงกับ JSON ที่ส่งมา
            if (!data.TryGetValue("user_name", out var user_name) ||
                !data.TryGetValue("password", out var password))
            {
                return BadRequest("Missing username or password");
            }

            var LoggedinUser = _context.Users.Where(userData =>
                userData.UserName == user_name
                && userData.Password == password);
            if (LoggedinUser.IsNullOrEmpty())
            {
                return NotFound();
            }
            return Ok(new { currentUser = LoggedinUser });
        }

        [HttpPost("register")]
        public async Task<IActionResult> PostUserAsync([FromBody] Dictionary<string, string> data)
        {
            // ดึงค่าออกมาจาก Dictionary โดยระบุ Key ให้ตรงกับ JSON ที่ส่งมา
            if (!data.TryGetValue("user_name", out var user_name) ||
                !data.TryGetValue("password", out var password))
            {
                return BadRequest("Missing username or password.");
            }

            if (!_context.Users.Where(userData => userData.UserName == user_name).IsNullOrEmpty())
            {
                return BadRequest("Already has this username.");
            }

            var newUser = new Users { UserName = user_name, Password = password }; //สร้าง user ใหม่ ไม่ต้องใส่ id เนื่องจากมี auto increment ใน database

            _context.Users.Add(newUser); //เพิ่ม user ลงในฐานข้อมูล

            await _context.SaveChangesAsync(); //save ลงฐานข้อมูล

            return Ok(new { currentUser = newUser });
        }

        [HttpPost("user-places/post")]
        public async Task<IActionResult> PostUserPlacesAsync([FromBody] UserFavoritePlaces favoritePlace)
        {
            _context.UserFavoritePlaces.Add(favoritePlace); 

            await _context.SaveChangesAsync(); //save ลงฐานข้อมูล
            return Ok();
        }

        [HttpDelete("user-places/delete")]
        public async Task<IActionResult> DeleteUserPlacesAsync(
            [FromQuery(Name = "user_id")] int user_id,
            [FromQuery(Name = "place_id")] int place_id)
        {
            var deletedCount = await _context.UserFavoritePlaces
                .Where(x => x.UserId == user_id && x.PlaceId == place_id) //เลือกที่ user_id และ place_id ตรงกัน
                .ExecuteDeleteAsync();

            if (deletedCount == 0)
            {
                return NotFound(); // ไม่มีข้อมูลถูกลบ
            }

            return Ok();
        }
    }
}
