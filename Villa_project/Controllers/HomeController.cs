using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Villa_project.Application.Common.Interfaces;
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
