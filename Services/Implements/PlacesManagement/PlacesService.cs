using System.Runtime.CompilerServices;
using Domain.Database;
using Domain.Database.Context;
using Domain.Exceptions;
using Domain.Interfaces.ImageManagement;
using Domain.Interfaces.PlacesManagement;
using Domain.ViewModels.PlacesManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Services.Implements.PlacesManagement
{
    public class PlacesService : IPlacesService
    {
        private readonly FavoritePlacesContext _context;
        private readonly IImageService _imageService;

        public PlacesService(FavoritePlacesContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }
        public List<Places> GetPlaces()
        {
            var places = _context.Places.ToList();

            return places;
            //throw new NotImplementedException();
        }

        public List<Places> GetAvailablePlaces()
        {
            var availablePlaces = _context.Places.Where(p => p.IsApproved == "Y" && p.IsDeleted == "N").ToList();

            return availablePlaces;
            //throw new NotImplementedException();
        }

        public List<Places> GetUserPlaces(int userId)
        {
            //var user = _context.Users.Find(userId);

            //if (user == null)
            //    throw new ValidateException("Invalid user.");

            var placeIds = _context.UserFavoritePlaces
                                   .Where(userPlace => userPlace.UserId == userId) // หาที่ user_id เหมือนกัน
                                   .Select(userPlace => userPlace.PlaceId) //เลือก place_id ทั้งหมดที่ user_id เหมือนกัน
                                   .ToList();

            var userPlaces = _context.Places
                                         .Where(p => placeIds.Contains(p.PlaceId)) //เลือก place ที่ place_id เหมือนกัน
                                         .ToList();

            return userPlaces;
        }

        public List<Places> GetUnapprovePlaces()
        {
            var unapprovePlaces = _context.Places.Where(p => p.IsApproved == "N").ToList();
            return unapprovePlaces;
        }

        public async Task<List<PlacesCommentViewModel>> GetPlacesComments()
        {
            var placesComments = await _context.PlacesComment
                .Select(c => new PlacesCommentViewModel
                {
                    PlaceId = c.PlaceId,
                    UserId = c.UserId,
                    Comment = c.Comment,
                    Rating = c.Rating,

                    UserName = c.User.UserName,
                    Title = c.Place.Title
                })
                .ToListAsync();

            return placesComments;
        }
        
        public async Task PostNewPlace(string title, string alt, string add_by, string isApproved, IFormFile formFile)
        {
            var result = await _imageService.UploadImageAsync(formFile);

            if (result == null)
                throw new ValidateException("No file uploaded.");

            var places = new Places
            {
                Title = title,
                ImgSrc = result.SecureUrl.AbsoluteUri,
                ImgAlt = alt,
                AddBy = add_by,
                IsApproved = isApproved
            };
            _context.Places.Add(places);
            _context.SaveChanges();
        }

        public async Task PostApproveUserAddedPlace(int placeId)
        {
            var place = _context.Places.Find(placeId);

            if (place != null)
            {
                //เปลี่ยนสถานะเป็นอนุมัติ
                place.IsApproved = "Y";

                await _context.SaveChangesAsync(); //save ลงฐานข้อมูล
            } else
            {
                throw new ValidateException("Invalid place.");
            }
        }

        public async Task PostUserPlace(UserFavoritePlaces userFavoritePlaces)
        {
            if (userFavoritePlaces == null)
            {
                throw new ValidateException("There is no place to add.");
            }
            _context.UserFavoritePlaces.Add(userFavoritePlaces);

            await _context.SaveChangesAsync(); //save ลงฐานข้อมูล
        }

        public async Task DeleteUserPlace(int userId, int placeId)
        {
            var placeToBeDeleted = _context.UserFavoritePlaces
                .Where(x => x.UserId == userId && x.PlaceId == placeId);

            if (placeToBeDeleted == null)
            {
                throw new KeyNotFoundException("There is no place to delete."); // ไม่มีข้อมูลถูกลบ
            }

            var deletedCount = await _context.UserFavoritePlaces
                .Where(x => x.UserId == userId && x.PlaceId == placeId) //เลือกที่ user_id และ place_id ตรงกัน
                .ExecuteDeleteAsync();

            if (deletedCount == 0)
            {
                throw new KeyNotFoundException("There is no place deleted."); // ไม่มีข้อมูลถูกลบ
            }
        }

        public async Task DeletePlace(int placeId)
        {
            var placeToBeDeleted = _context.Places.Find(placeId);

            if (placeToBeDeleted != null)
            {
                placeToBeDeleted.IsDeleted = "Y";
                await _context.SaveChangesAsync();
            } else
            {
                throw new KeyNotFoundException("There is no place to delete.");
            }
        }
    }
}
