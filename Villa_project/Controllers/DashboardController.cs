using Microsoft.AspNetCore.Mvc;
using Villa_project.Application.Common.Interfaces;
using Villa_project.Application.Common.Utility;
using Villa_project.Infrastructure.Repository;
using Villa_project.ViewModels;

namespace Villa_project.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfwork;
        static int previousMonth = DateTime.Now.Month==1 ? 12 : DateTime.Now.Month-1;
        readonly DateTime previousMonthStartDate = new DateTime(DateTime.Now.Year, previousMonth, 1);
        readonly DateTime currentMonthStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month , 1);
        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfwork=unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTotalBookingRadialChartData()
        {
            var totalbooking = _unitOfwork.Booking.GetAll(u => u.Status!=SD.StatusPending
            || u.Status==SD.StatusCancelled);

            var countByCurrentMonth = totalbooking.Count(u => u.BookingDate>=currentMonthStartDate &&
            u.BookingDate<=DateTime.Now);

            var countByPreviousMonth = totalbooking.Count(u => u.BookingDate>=previousMonthStartDate &&
          u.BookingDate<=currentMonthStartDate);

            return Json(GetRadialChartDataModel(totalbooking.Count(), countByCurrentMonth, countByPreviousMonth));
        }

        public async Task<IActionResult> GetRegisteredUserChartData()
        {
            var totalUser = _unitOfwork.User.GetAll();

            var countByCurrentMonth = totalUser.Count(u => u.CreatedAt>=currentMonthStartDate &&
            u.CreatedAt<=DateTime.Now);

            var countByPreviousMonth = totalUser.Count(u => u.CreatedAt>=previousMonthStartDate &&
          u.CreatedAt<=currentMonthStartDate);

          

            return Json(GetRadialChartDataModel(totalUser.Count(),countByCurrentMonth,countByPreviousMonth));

        }

        public async Task<IActionResult> GetRevenueChartData()
        {

            var totalbooking = _unitOfwork.Booking.GetAll(u => u.Status!=SD.StatusPending
           || u.Status==SD.StatusCancelled);

            var totalRevenue =Convert.ToInt32(totalbooking.Sum(u => u.TotalCost));


            var totalUser = _unitOfwork.User.GetAll();

            var countByCurrentMonth = totalbooking.Where(u => u.BookingDate>=currentMonthStartDate &&
u.BookingDate<=DateTime.Now).Sum(u=>u.TotalCost);

            var countByPreviousMonth = totalbooking.Where(u => u.BookingDate>=previousMonthStartDate &&
          u.BookingDate<=currentMonthStartDate).Sum(u => u.TotalCost);



            return Json(GetRadialChartDataModel(totalRevenue, countByCurrentMonth, countByPreviousMonth));

        }


        public async Task<IActionResult> GetBookingPieChartData()
        {
            var totalbooking = _unitOfwork.Booking.GetAll(  u => u.BookingDate>=DateTime.Now.AddDays(-30) && ( u.Status!=SD.StatusPending
            || u.Status==SD.StatusCancelled));


            var customerWithOneBooking = totalbooking.GroupBy(u => u.UserId).Where(x => x.Count()==1).Select(x=>x.Key).ToList();

            int bookingsByNewCustomer = customerWithOneBooking.Count();
            int bookingsByReturningCustomer = totalbooking.Count()-bookingsByNewCustomer;

            PieChartVM pieChartVM = new()
            {
                Labels=new string[] { "New Customers", "Returning Customer" },
                Series=new decimal[] { bookingsByNewCustomer, bookingsByReturningCustomer }
            };

           
            return Json(pieChartVM);
        }

        public async Task<LineChartVM> GetMemberAndBookingLineChartData()
        {
            var bookingData = _unitOfwork.Booking.GetAll(u => u.BookingDate >= DateTime.Now.AddDays(-30) &&
             u.BookingDate.Date <= DateTime.Now)
                 .GroupBy(b => b.BookingDate.Date)
                 .Select(u => new {
                     DateTime = u.Key,
                     NewBookingCount = u.Count()
                 });

            var customerData = _unitOfwork.User.GetAll(u => u.CreatedAt >= DateTime.Now.AddDays(-30) &&
            u.CreatedAt.Date <= DateTime.Now)
                .GroupBy(b => b.CreatedAt.Date)
                .Select(u => new {
                    DateTime = u.Key,
                    NewCustomerCount = u.Count()
                });


            var leftJoin = bookingData.GroupJoin(customerData, booking => booking.DateTime, customer => customer.DateTime,
                (booking, customer) => new
                {
                    booking.DateTime,
                    booking.NewBookingCount,
                    NewCustomerCount = customer.Select(x => x.NewCustomerCount).FirstOrDefault()
                });


            var rightJoin = customerData.GroupJoin(bookingData, customer => customer.DateTime, booking => booking.DateTime,
                (customer, booking) => new
                {
                    customer.DateTime,
                    NewBookingCount = booking.Select(x => x.NewBookingCount).FirstOrDefault(),
                    customer.NewCustomerCount
                });

            var mergedData = leftJoin.Union(rightJoin).OrderBy(x => x.DateTime).ToList();

            var newBookingData = mergedData.Select(x => x.NewBookingCount).ToArray();
            var newCustomerData = mergedData.Select(x => x.NewCustomerCount).ToArray();
            var categories = mergedData.Select(x => x.DateTime.ToString("MM/dd/yyyy")).ToArray();

            List<ChartData> chartDataList = new()
            {
                new ChartData
                {
                    Name = "New Bookings",
                    Data = newBookingData
                },
                new ChartData
                {
                    Name = "New Members",
                    Data = newCustomerData
                },
            };

            LineChartVM lineChart = new()
            {
                Categories = categories,
                Series = chartDataList
            };

            return lineChart;
        }


        private static RadialBarChartVM GetRadialChartDataModel(int totalCount,double currentMonthCount,double prevMonthCount)
        {
            RadialBarChartVM radialBarChartVM = new();

            int increaseDecreaseRatio = 100;
            if (prevMonthCount!=0)
            {
                increaseDecreaseRatio=Convert.ToInt32((currentMonthCount-prevMonthCount)/ prevMonthCount* 100);

            }

            radialBarChartVM.TotalCount=totalCount;
            radialBarChartVM.CountInCurrentMonth=Convert.ToInt32(currentMonthCount);
            radialBarChartVM.HasRatioIncreased= currentMonthCount>prevMonthCount;
            radialBarChartVM.Series=new int[] { increaseDecreaseRatio };

            return radialBarChartVM;
        }
    }
}

