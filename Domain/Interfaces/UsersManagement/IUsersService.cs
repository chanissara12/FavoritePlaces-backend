using Domain.Database;
using Domain.ViewModels.UsersManagement;

namespace Domain.Interfaces.UsersManagement
{
    public interface IUsersService
    {
        List<Users> GetUsers();

        Task<ReturnLoggedInUserViewModel> LoginUser(UserLoginViewModel userLoginViewModel);

        Task<ReturnLoggedInUserViewModel> RegisterUser(UserLoginViewModel userLoginViewModel);
    }
}
