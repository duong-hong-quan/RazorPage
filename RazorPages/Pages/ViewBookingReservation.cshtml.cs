using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.Helper;
using Repository.Interface;

namespace RazorPages.Pages
{
    public class ViewBookingReservationModel : PageModel
    {
        private readonly IBookingReservationRepository _bookingReservationRepository;
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }

        [BindProperty]
        public ICollection<BookingReservation> Bookings { get; set; }

        [BindProperty]
        public string Message { get; set; }

        public ViewBookingReservationModel(IBookingReservationRepository bookingReservationRepository)
        {
            _bookingReservationRepository = bookingReservationRepository;
        }

        public async Task<IActionResult> OnGet()
        {
            Customer = SessionHelper.GetObjectFromJson<Customer>(HttpContext.Session, "Customer");
            CustomerId = Customer.CustomerId;
            var bookings = await _bookingReservationRepository.ViewAllBookingReservationByUserId(CustomerId);
            if (bookings.Data == null)
            {
                Bookings = new List<BookingReservation>();
                Message = bookings.Message;
                return Page();
            }
            else
            {
                Bookings = bookings.Data;
                Message = bookings.Message;
                return Page();
            }
        }

        public void OnPost()
        {
        }
    }
}