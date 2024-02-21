using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Interface;

namespace RazorPages.Pages.BookingReservations
{
    public class CreateModel : PageModel
    {
        private readonly IBookingReservationRepository _bookingReservationRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IRoomInformationRepository _roomInformationRepository;

        public CreateModel(IBookingReservationRepository bookingReservationRepository, ICustomerRepository customerRepository, IRoomInformationRepository roomInformationRepository)
        {
            _bookingReservationRepository = bookingReservationRepository;
            _customerRepository = customerRepository;
            _roomInformationRepository = roomInformationRepository;
        }

        public async Task<IActionResult> OnGet()
        {
            var customers = await _customerRepository.GetAllCustomers();
            ViewData["CustomerId"] = new SelectList(customers.Where(x => x.CustomerStatus == 1), "CustomerId", "EmailAddress");
            var rooms = await _roomInformationRepository.GetAllRooms();
            ViewData["RoomId"] = new SelectList(rooms.Where(x => x.RoomStatus == 1), "RoomId", "RoomNumber");
            return Page();
        }

        [BindProperty]
        public BookingReservation BookingReservation { get; set; } = default!;

        [BindProperty]
        public BookingDetail BookingDetail { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string ErrorMsg { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var checkCreateBooking = await _bookingReservationRepository.Booking(BookingReservation.CustomerId, BookingDetail.RoomId, BookingDetail.StartDate, BookingDetail.EndDate);
            if (checkCreateBooking.isSuccess)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                ErrorMsg = checkCreateBooking.Message;
                return Page();
            }
        }
    }
}