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
                VillaNumber = new VillaNumber(), // ensures model binding works properly
                Villalist = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };

            return View(villaNumberVM);
      
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {

           bool exist = _db.VillaNumbers.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);
            if (obj == null) return BadRequest();

             ModelState.Remove("Villa");
             if (ModelState.IsValid && !exist)
             {
                 _db.VillaNumbers.Add(obj.VillaNumber);
                 _db.SaveChanges();
                 return RedirectToAction("Index");
             }
           

            if (exist)
            {
                TempData["error"]="Villa already exist ";
            }


            obj.Villalist=_db.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

             return View(obj);
           
        }


        public IActionResult Update(int villaId)
        {
            VillaNumberVM villaNumberVM = new()
            {

                Villalist = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),

                VillaNumber=_db.VillaNumbers.FirstOrDefault(u => u.Villa_Number==villaId)
            };
            if(villaNumberVM.VillaNumber==null)
            {
                return NotFound();
            }

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Update(villaNumberVM.VillaNumber);
                _db.SaveChanges();
                TempData["success"]="The villa have been updated";
                return RedirectToAction("Index");
            }
            villaNumberVM.Villalist=_db.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(villaNumberVM);
        }

        public IActionResult Delete(int villaId)
        {
            VillaNumberVM villaNumberVM = new()
            {

                Villalist = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),

                VillaNumber=_db.VillaNumbers.FirstOrDefault(u => u.Villa_Number==villaId)
            };
            if (villaNumberVM.VillaNumber==null)
            {
                return NotFound();
            }

            return View(villaNumberVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken] // recommended for security
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            var villaInDb = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

            if (villaInDb != null)
            {
                _db.VillaNumbers.Remove(villaInDb);
                _db.SaveChanges();
                TempData["success"] = "The villa has been deleted";
                return RedirectToAction("Index");
            }

            TempData["error"] = "The villa deletion failed";
            return View(villaNumberVM);
        }


    }
}
