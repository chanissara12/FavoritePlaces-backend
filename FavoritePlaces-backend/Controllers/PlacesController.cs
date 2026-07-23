
using Azure.Core;
using Domain.Database;
using Domain.Database.Context;
using Domain.Exceptions;
using Domain.Interfaces.ImageManagement;
using Domain.Interfaces.PlacesManagement;
using Domain.ViewModels.PlacesManagement;
using Microsoft.AspNetCore.Mvc;

namespace FavoritePlacesApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly FavoritePlacesContext _context;
        private readonly IImageService _imageService;
        private readonly IPlacesService _placesService;

        public PlacesController(FavoritePlacesContext context, IImageService imageService, IPlacesService placesService)
        {
            _context = context;
            _imageService = imageService;
            _placesService = placesService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Places>> Get()
        {
            var places = _context.Places.ToList();

            return Ok(new { places = places });
        }

        [HttpGet("GetAvailablePlaces")]
        public async Task<IActionResult> GetAvailablePlaces(int userId)
        //public ActionResult<IEnumerable<PlacesViewModel>> GetAvailablePlaces(int userId)
        {
            var availablePlaces = await _placesService.GetAvailablePlaces(userId);

            return Ok(new { places = availablePlaces });
        }

        [HttpGet("GetUserPlaces")]
        public ActionResult<IEnumerable<PlacesViewModel>> GetUserPlaces(int userId)
        {
            var userPlaces = _placesService.GetUserPlaces(userId);

            return Ok(new { places = userPlaces });
        }

        [HttpGet("unapprove-places")]
        public ActionResult<IEnumerable<UserFavoritePlaces>> GetUnapprovePlaces()
        {
            var unapprovePlaces = _placesService.GetUnapprovePlaces();

            return Ok(new { places = unapprovePlaces });
        }

        [HttpGet("places-comments")]
        public async Task<IActionResult> GetPlacesComments(int placeId)
        {
            var placesComments = await _placesService.GetPlacesComments(placeId);

            return Ok(new { comments = placesComments });
        }

        [HttpPost("new-place")]
        public async Task<IActionResult> PostNewPlaces(
            [FromForm] string title, 
            [FromForm] string alt, 
            [FromForm] string add_by, 
            [FromForm] string isApproved, 
            [FromForm] string uploadedUserId, 
            [FromForm] IFormFile formFile)
        {
            await _placesService.PostNewPlace(title, alt, add_by, isApproved, uploadedUserId, formFile);

            return Ok();
        }

        [HttpPost("user-places/approve-place")]
        public async Task<IActionResult> AprroveUserAvailablePlacesAsync([FromBody] int placeId)
        {
            await _placesService.PostApproveUserAddedPlace(placeId);

            return Ok();
        }

        [HttpPost("user-places/post")]
        public async Task<IActionResult> PostUserPlacesAsync([FromBody] UserFavoritePlaces favoritePlace)
        {
            await _placesService.PostUserPlace(favoritePlace);

            return Ok();
        }

        [HttpDelete("user-places/delete")]
        public async Task<IActionResult> DeleteUserPlacesAsync(
            [FromQuery(Name = "userId")] int userId,
            [FromQuery(Name = "placeId")] int placeId)
        {
            await _placesService.DeleteUserPlace(userId, placeId);

            return Ok();
        }

        [HttpPost("delete-place")]
        public async Task<IActionResult> DeletePlace([FromBody] int placeId)
        {
            await _placesService.DeletePlace(placeId);

            return Ok();
        }
    }
}
