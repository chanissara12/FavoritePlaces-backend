
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

        [HttpGet("available-places")]
        public ActionResult<List<Places>> GetAvailablePlaces()
        {
            var availablePlaces = _placesService.GetAvailablePlaces();

            return Ok(new { places = availablePlaces });
        }

        [HttpGet("user-places")]
        public ActionResult<IEnumerable<UserFavoritePlaces>> GetUserPlaces(int userId)
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
        public async Task<IActionResult> GetPlacesComments()
        {
            var placesComments = await _placesService.GetPlacesComments();

            return Ok(new { comments = placesComments });
        }

        [HttpPost("new-place")]
        public async Task<IActionResult> PostNewPlaces(
            [FromForm] string title, 
            [FromForm] string alt, 
            [FromForm] string add_by, 
            [FromForm] string isApproved, 
            [FromForm] IFormFile formFile)
        {
            try
            {
                await _placesService.PostNewPlace(title, alt, add_by, isApproved, formFile);

                return Ok();
            } catch (ValidateException ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [HttpPost("user-places/approve-place")]
        public async Task<IActionResult> AprroveUserAvailablePlacesAsync([FromBody] int placeId)
        {
            try
            {
                await _placesService.PostApproveUserAddedPlace(placeId);

                return Ok();
            }
            catch (ValidateException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("user-places/post")]
        public async Task<IActionResult> PostUserPlacesAsync([FromBody] UserFavoritePlaces favoritePlace)
        {
            try
            {
                await _placesService.PostUserPlace(favoritePlace);

                return Ok();
            }
            catch (ValidateException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("user-places/delete")]
        public async Task<IActionResult> DeleteUserPlacesAsync(
            [FromQuery(Name = "userId")] int userId,
            [FromQuery(Name = "placeId")] int placeId)
        {
            try
            {
                await _placesService.DeleteUserPlace(userId, placeId);

                return Ok();
            }
            catch (ValidateException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("delete-place")]
        public async Task<IActionResult> DeletePlace([FromBody] int placeId)
        {
            try
            {
                await _placesService.DeletePlace(placeId);

                return Ok();
            }
            catch (ValidateException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
