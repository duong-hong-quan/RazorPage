using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace RazorPages.Pages.BookingReservations
{
    public class DetailsModel : PageModel
    {
        private readonly DataAccessLayer.FuminiHotelManagementContext _context;

        public DetailsModel(DataAccessLayer.FuminiHotelManagementContext context)
        {
            _context = context;
        }

        public BookingReservation BookingReservation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.BookingReservations == null)
            {
                return NotFound();
            }

            var bookingreservation = await _context.BookingReservations.FirstOrDefaultAsync(m => m.BookingReservationId == id);
            if (bookingreservation == null)
            {
                return NotFound();
            }
            else
            {
                BookingReservation = bookingreservation;
            }
            return Page();
        }
    }
}