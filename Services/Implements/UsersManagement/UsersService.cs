using Domain.Database;
using Domain.Database.Context;
using Domain.Exceptions;
using Domain.Interfaces.UsersManagement;
using Domain.ViewModels.UsersManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Services.Implements.UsersManagement
{
    public class UsersService : IUsersService
    {
        private readonly FavoritePlacesContext _context;

        public UsersService(FavoritePlacesContext context)
        {
            _context = context;
        }
        public List<Users> GetUsers()
        {
            return _context.Users.ToList();
        }

        public async Task<ReturnLoggedInUserViewModel> LoginUser([FromBody] UserLoginViewModel request)
        {
            // หาตัวแรกที่ตรงกัน
            var LoggedinUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == request.userName && u.Password == request.password);

            //เช็คว่ามี user ในระบบไหม
            if (LoggedinUser == null)
            {
                throw new ValidateException("Invalid Username or Password");
            }

            // ดึงข้อมูล role
            var userRole = await _context.UserRole
                .Where(ur => ur.UserId == LoggedinUser.UserId)
                .Select(ur => ur.RoleId)
                .ToListAsync(); //ทำให้เป็น list

            var currentUser = new ReturnLoggedInUserViewModel { 
                userId = LoggedinUser.UserId, 
                userName = LoggedinUser.UserName, 
                roles = userRole 
            };

            return currentUser;
        }

        public async Task<ReturnLoggedInUserViewModel> RegisterUser([FromBody] UserLoginViewModel request)
        {
            //เช็คว่ามี user ตามเงื่อนไขไหม
            if (_context.Users.Any(u => u.UserName == request.userName))
            {
                throw new ValidateException("Already have this username. Please change username and try again.");
            }

            var newUser = new Users { UserName = request.userName, Password = request.password };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync(); // save user ก่อนเพื่อสร้าง userId

            var newUserRole = new UserRole { 
                UserId = newUser.UserId, 
                RoleId = "user", 

            };

            _context.UserRole.Add(newUserRole); //เพิ่ม role

            await _context.SaveChangesAsync(); //save ลงฐานข้อมูล

            var currentUser = new ReturnLoggedInUserViewModel
            {
                userId = newUser.UserId,
                userName = newUser.UserName,
                roles = [newUserRole.RoleId]
            };

            return currentUser;
        }
    }
}
