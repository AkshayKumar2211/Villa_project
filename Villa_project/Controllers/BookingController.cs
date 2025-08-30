using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Security.Claims;
using Villa_project.Application.Common.Interfaces;
using Villa_project.Domain.Entities;

namespace Villa_project.Controllers
{
    public class BookingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        public IActionResult FinalizeBooking(int villaId, DateOnly checkInDate, int nights)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ApplicationUser user = _unitOfWork.User.Get(u => u.Id==userId);

            Booking booking = new()
            {
                VillaId = villaId,
                Villa = _unitOfWork.villa.Get(u=>u.Id==villaId,includeProperties:"VillaAmenity"),
                CheckInDate = checkInDate,
                Nights = nights,
                CheckOutDate = checkInDate.AddDays(nights),
                UserId = userId,
                Phone=user.PhoneNumber,
               Email=user.Email,
                Name=user.Name
            };
            booking.TotalCost = booking.Villa.Price * nights;
            return View(booking);
        }
    }
}
