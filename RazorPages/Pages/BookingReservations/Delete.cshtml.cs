using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Interface;

namespace RazorPages.Pages.BookingReservations
{
    public class DeleteModel : PageModel
    {
        private readonly IBookingReservationRepository _bookingReservationRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IRoomInformationRepository _roomInformationRepository;

        public DeleteModel(IBookingReservationRepository bookingReservationRepository, ICustomerRepository customerRepository, IRoomInformationRepository roomInformationRepository)
        {
            _bookingReservationRepository = bookingReservationRepository;
            _customerRepository = customerRepository;
            _roomInformationRepository = roomInformationRepository;
        }

        [BindProperty]
        public BookingReservation BookingReservation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var bookingReservation = await _bookingReservationRepository.GetById(id);

            if (bookingReservation == null)
            {
                return NotFound();
            }
            else
            {
                BookingReservation = bookingReservation;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var bookingreservation = await _bookingReservationRepository.GetById(id);

            if (bookingreservation != null)
            {
                var check = await _bookingReservationRepository.Deletebooking(bookingreservation.BookingReservationId);
                if (check > 0)
                {
                    return RedirectToPage("./Index");
                }
            }

            return Page();
        }
    }
}