using Domain.Database;
using Microsoft.AspNetCore.Http;

namespace Domain.Interfaces.PlacesManagement
{
    public interface IPlacesService
    {
        List<Places> GetPlaces();

        List<Places> GetAvailablePlaces();

        List<Places> GetUserPlaces(int userId);

        List<Places> GetUnapprovePlaces();

        List<PlacesComment> GetPlacesComments();

        Task PostNewPlace(string title, string alt, string add_by, string isApprove, IFormFile formFile);

        Task PostApproveUserAddedPlace(int placeId);

        Task PostUserPlace(UserFavoritePlaces userFavoritePlaces);

        Task DeleteUserPlace(int userId, int placeId);

        Task DeletePlace(int placeId);
    }
}
