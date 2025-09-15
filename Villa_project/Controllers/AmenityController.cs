using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Villa_project.Application.Common.Interfaces;
using Villa_project.Application.Common.Utility;
using Villa_project.Domain.Entities;
using Villa_project.ViewModels;

namespace Villa_project.Controllers
{
    [Authorize(Roles =SD.Role_Admin)]
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;


        public AmenityController(IUnitOfWork unit)
        {
            _unitOfWork=unit;
        }
        public IActionResult Index()
        {
            var amentiesInDb = _unitOfWork.amenity.GetAll(includeProperties: "Villa");
            return View(amentiesInDb);
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

            AmenityVM amenityVM = new()
            {
                Amenity = new Amenity(), // ensures model binding works properly
                Villalist = _unitOfWork.villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };

            return View(amenityVM);

        }

        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {

    
            if (obj == null) return BadRequest();

            ModelState.Remove("Villa");
            if (ModelState.IsValid )
            {
                _unitOfWork.amenity.Add(obj.Amenity);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }


           


            obj.Villalist= _unitOfWork.villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(obj);

        }


        public IActionResult Update(int amenityId)
        {
           AmenityVM amenityVM = new()
            {

                Villalist =  _unitOfWork.villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),

                Amenity=_unitOfWork.amenity.Get(u => u.Id==amenityId)
            };
            if (amenityVM.Amenity==null)
            {
                return NotFound();
            }

            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Update(AmenityVM amenityVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.amenity.Update(amenityVM.Amenity);
                _unitOfWork.Save();
                TempData["success"]="The villa have been updated";
                return RedirectToAction("Index");
            }
            amenityVM.Villalist= _unitOfWork.villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(amenityVM);
        }

        public IActionResult Delete(int amenityId)
        {
            AmenityVM amenityVM = new()
            {

                Villalist =  _unitOfWork.villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),

                Amenity=_unitOfWork.amenity.Get(u => u.Id==amenityId)
            };
            if (amenityVM.Amenity==null)
            {
                return NotFound();
            }

            return View(amenityVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken] // recommended for security
        public IActionResult Delete(AmenityVM amenityVM)
        {
            Amenity? amenityInDb = _unitOfWork.amenity.Get(u => u.Id == amenityVM.Amenity.Id);

            if (amenityInDb != null)
            {
                _unitOfWork.amenity.Remove(amenityInDb);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been deleted";
                return RedirectToAction("Index");
            }

            TempData["error"] = "The villa deletion failed";
            return View(amenityVM);
        }
    }
}
