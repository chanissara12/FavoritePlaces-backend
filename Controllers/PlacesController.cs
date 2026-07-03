using FavoritePlacesApi.Data;
using FavoritePlacesApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace FavoritePlacesApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public PlacesController(ApplicationDBContext context)
        {
            _context = context; 
        }

        [HttpGet]
        public ActionResult<IEnumerable<Places>> Get()
        {
            var places = _context.Places.ToList();

            return Ok(new { places = places });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Create(Places obj)
        {
            _context.Places.Add(obj);
            _context.SaveChanges();
        }

        //// GET: PlacesController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: PlacesController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: PlacesController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: PlacesController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: PlacesController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: PlacesController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: PlacesController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
