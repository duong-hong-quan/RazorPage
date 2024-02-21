using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Interface;

namespace RazorPages.Pages.Rooms
{
    public class EditModel : PageModel
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IRoomInformationRepository _roomInformationRepository;

        public EditModel(IRoomTypeRepository roomTypeRepository, IRoomInformationRepository roomInformationRepository)
        {
            _roomTypeRepository = roomTypeRepository;
            _roomInformationRepository = roomInformationRepository;
        }

        public ICollection<RoomType> RoomTypes { get; set; }

        [BindProperty]
        public RoomInformation RoomInformation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            RoomTypes = await _roomTypeRepository.GetAllRommTypes();
            var roominformation = await _roomInformationRepository.GetRoomById(id);
            ; if (roominformation == null)
            {
                return NotFound();
            }
            RoomInformation = roominformation;
            ViewData["RoomTypeId"] = new SelectList(RoomTypes, "RoomTypeId", "RoomTypeName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var room = await _roomInformationRepository.UpdateRoom(RoomInformation);
            return RedirectToPage("./Index");
        }

        /*        private bool RoomInformationExists(int id)
                {
                  return (_context.RoomInformations?.Any(e => e.RoomId == id)).GetValueOrDefault();
                }*/
    }
}