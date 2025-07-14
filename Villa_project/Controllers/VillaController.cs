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

            var villaInDb = _db.Villas.Find(villaId);
            return View(villaInDb);
        }
        [HttpPost]
        public IActionResult Edit(Villa villa)
        {
            if (!ModelState.IsValid) return NotFound();

            _db.Villas.Update(villa);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Villa villa)
        {
            if (!ModelState.IsValid) return NotFound();

            _db.Villas.Add(villa);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        /*public IActionResult Delete(int villaId)
        {
            if (villaId==null) return NotFound();

            var villaInDb = _db.Villas.Find(villaId);

            return View(villaInDb);
          
        }

        [HttpPost]
        public IActionResult Delete(int villaId)
        {
            if (!ModelState.IsValid) return NotFound();

            var villaInDb = _db.Villas.Find(villaId);

            
            _db.Villas.Remove(villaInDb);


            _db.SaveChanges();
            return RedirectToAction("Index");
        } */
    }
}
