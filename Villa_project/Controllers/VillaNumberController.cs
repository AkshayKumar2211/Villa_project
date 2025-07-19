using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using Villa_project.Domain.Entities;
using Villa_project.Infrastructure.Data;
using Villa_project.ViewModels;

namespace Villa_project.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;
        

        public VillaNumberController(ApplicationDbContext db)
        {
            _db=db;
        }
        public IActionResult Index()
        {
            var villasNumberInDb = _db.VillaNumbers.Include(u=>u.Villa).ToList();
            return View(villasNumberInDb);
        }

        public IActionResult Create()
        {
            /* IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(
                 u => new SelectListItem
                 {
                     Text = u.Name, // this will be options in drop down
                     Value=u.Id.ToString()
                 }
                 );

             ViewData["Villa List"]=list;*/

            VillaNumberVM villaNumberVM = new()
            {
                Villalist=_db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text=u.Name,
                    Value=u.Id.ToString()
                }
               )
            };
            return View(villaNumberVM);
      
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM villaNumber)
        {
            bool exist = _db.VillaNumbers.Any(u => u.Villa_Number==villaNumber.VillaNumber.Villa_Number);
            if (exist)
            {
                TempData["error"]="Data is already in database";

            }
            if (villaNumber == null) return BadRequest();

             ModelState.Remove("Villa");
             if (ModelState.IsValid)
             {
                 _db.VillaNumbers.Add(villaNumber.VillaNumber);
                 _db.SaveChanges();
                 return RedirectToAction("Index");
             }
             return View(villaNumber);
           
        }
    }
}
