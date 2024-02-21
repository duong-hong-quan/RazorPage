using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Interface;

namespace RazorPages.Pages.BookingReservations
{
    public class IndexModel : PageModel
    {
        private readonly IBookingReservationRepository _bookingReservationRepository;

        public IndexModel(IBookingReservationRepository bookingReservationRepository)
        {
            _bookingReservationRepository = bookingReservationRepository;
        }

        public IList<BookingReservation> BookingReservation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            BookingReservation = await _bookingReservationRepository.GetAll();
            return Page();
        }
    }
}