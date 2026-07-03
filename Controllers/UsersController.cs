using FavoritePlacesApi.Data;
using FavoritePlacesApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data.Entity;

namespace FavoritePlacesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public UsersController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Users>> Get()
        {
            var users = _context.Users.ToList();

            return Ok(new { users = users });
        }

        [HttpGet("user-places")]
        public ActionResult<IEnumerable<UserFavoritePlaces>> GetUserPlaces(int user_id)
        {
            var placeIds = _context.UserFavoritePlaces
                                   .Where(userPlace => userPlace.user_id == user_id) // หาที่ user_id เหมือนกัน
                                   .Select(userPlace => userPlace.place_id) //เลือก place_id ทั้งหมดที่ user_id เหมือนกัน
                                   .ToArray();

            var userPlaces = _context.Places
                                         .Where(p => placeIds.Contains(p.place_id)) //เลือก place ที่ place_id เหมือนกัน
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
                userData.user_name == user_name
                && userData.password == password);
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

            if (!_context.Users.Where(userData => userData.user_name == user_name).IsNullOrEmpty())
            {
                return BadRequest("Already has this username.");
            }

            var newUser = new Users { user_name = user_name, password = password }; //สร้าง user ใหม่ ไม่ต้องใส่ id เนื่องจากมี auto increment ใน database

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
                .Where(x => x.user_id == user_id && x.place_id == place_id) //เลือกที่ user_id และ place_id ตรงกัน
                .ExecuteDeleteAsync();

            if (deletedCount == 0)
            {
                return NotFound(); // ไม่มีข้อมูลถูกลบ
            }

            return Ok();
        }
    }
}
