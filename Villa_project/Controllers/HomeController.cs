using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Diagnostics;
using Villa_project.Application.Common.Interfaces;
using Villa_project.Application.Common.Utility;
using Villa_project.Models;
using Villa_project.ViewModels;

namespace Villa_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                VillaList=_unitOfWork.villa.GetAll(includeProperties:"VillaAmenity"),  
                Nights=1,
                CheckInDate=DateOnly.FromDateTime(DateTime.Now),


            };
            return View(homeVM);
        }

        [HttpPost]
        public IActionResult GetVillasByDate(int nights,DateOnly CheckInDate)
        {
          //  Thread.Sleep(2000); // this for loading spinner
            var villaList = _unitOfWork.villa.GetAll(includeProperties: "VillaAmenity").ToList();
            var villaNumberList = _unitOfWork.villaNumber.GetAll().ToList();
            var bookedVillas=_unitOfWork.Booking.GetAll(u=>u.Status==SD.StatusApproved ||
            u.Status==SD.StatusCheckedIn).ToList();
           foreach(var villa in villaList)
            {
                int roomAvailable = SD.VillaRoomsAvailable_Count(villa.Id,villaNumberList, CheckInDate,nights,bookedVillas);

                villa.IsAvailable = roomAvailable > 0?true:false;

            }

            HomeVM homeVM = new()
            {
                CheckInDate=CheckInDate,
                VillaList=villaList,
                Nights=nights
            };

            return PartialView("_VillaList", homeVM);
        }


  


        public IActionResult Privacy()
        {
            return View();
        }

    
        public IActionResult Error()
        {
            return View();
        }
    }
}


