using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Villa_project.Application.Common.Interfaces;
using Villa_project.Application.Common.Utility;
using Villa_project.Domain.Entities;
using Villa_project.Infrastructure.Data;

namespace Villa_project.Infrastructure.Repository
{
    public class BookingRepository:Repository<Booking>,IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        public BookingRepository(ApplicationDbContext context):base(context) 
        {
            _context = context;
        }

        public void Update(Booking booking)
        {
            _context.Bookings.Update(booking);
        }

        public void UpdateStatus(int bookingId, string bookingStatus,int villaNumber=0)
        {
            var bookingFromDb = _context.Bookings.FirstOrDefault(m => m.Id==bookingId);
            if(bookingFromDb != null)
            {
                bookingFromDb.Status= bookingStatus;
                if(bookingStatus==SD.StatusCheckedIn)
                {
                    bookingFromDb.VillaNumber=villaNumber;
                    bookingFromDb.ActualCheckInDate= DateTime.Now;
                }

                if(bookingStatus==SD.StatusCompleted)
                {
                   bookingFromDb.ActualCheckOutDate= DateTime.Now;
                }
            }
        }

        public void UpdateStripePaymentID(int bookingId, string sessionId, string paymentIntentId)
        {
            var bookingFromDb = _context.Bookings.FirstOrDefault(m => m.Id==bookingId);
            if (bookingFromDb != null)
            {
                if(!string.IsNullOrEmpty(sessionId))
                {
                    bookingFromDb.StripeSessionId=sessionId;
                }
                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    bookingFromDb.StripePaymentIntentId=paymentIntentId;
                    bookingFromDb.PaymentDate= DateTime.Now;
                    bookingFromDb.IsPaymentSuccessful=true;
                }
            }

        }
    }
}
