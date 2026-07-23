using Domain.Database;
using Domain.ViewModels.PlacesManagement;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces.PlacesManagement
{
    public interface IPlacesService
    {
        List<Places> GetPlaces();

        Task<List<PlacesViewModel>> GetAvailablePlaces(int? userId);

        List<PlacesViewModel> GetUserPlaces(int userId);

        List<Places> GetUnapprovePlaces();

        Task<List<PlacesCommentViewModel>> GetPlacesComments(int placeId);

        Task PostNewPlace(string title, string alt, string add_by, string isApprove, string uploadedUserId, IFormFile formFile);

        Task PostApproveUserAddedPlace(int placeId);

        Task PostUserPlace(UserFavoritePlaces userFavoritePlaces);

        Task DeleteUserPlace(int userId, int placeId);

        Task DeletePlace(int placeId);
    }
}
