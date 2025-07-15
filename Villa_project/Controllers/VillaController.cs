using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Villa_project.Domain.Entities;
using Villa_project.Infrastructure.Data;

namespace Villa_project.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaController(ApplicationDbContext db)
        {
            _db=db;
        }

        public IActionResult Index()
        {
            var villas = _db.Villas.ToList();
            return View(villas);
        }

        public IActionResult Edit(int villaId)
        {
            if (villaId==null) return View();

            //var villaInDb = _db.Villas.Find(villaId);
            // for filter we use where
            //_db.Villas.Where(u => u.Id>4);

            var villaInDb=_db.Villas.FirstOrDefault(u=>u.Id==villaId);
            if(villaInDb==null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(villaInDb);
        }
        [HttpPost]
        public IActionResult Edit(Villa villa)
        {

            if (ModelState.IsValid)
            {
                _db.Villas.Update(villa);
                _db.SaveChanges();
                TempData["success"]="The data have been updated";
                return RedirectToAction("Index");
            }
            TempData["error"]="The data have not been updated";
            return View(villa);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Villa villa)
        {

            if(villa.Name==villa.Description)
            {
                ModelState.AddModelError("name", "The name and description cannot be same");
            }


            if (ModelState.IsValid)
            {
                _db.Villas.Add(villa);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(villa);
        }

        public IActionResult Delete(int villaId)
        {
            if (villaId==null) return NotFound();

            var villaInDb = _db.Villas.Find(villaId);

            return View(villaInDb);
          
        }

        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            
            var villaInDb = _db.Villas.Find(villa.Id);

            if (villaInDb is not null)
            {
                _db.Villas.Remove(villaInDb);
                _db.SaveChanges();
                TempData["Success"]="The data has been deleted";
                return RedirectToAction("Index");
            }

            TempData["error"]="The data has not been deleted";
            return View();
        } 


    }
}
