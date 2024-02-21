using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Interface;

namespace RazorPages.Pages.Rooms
{
    public class DeleteModel : PageModel
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IRoomInformationRepository _roomInformationRepository;

        public DeleteModel(IRoomTypeRepository roomTypeRepository, IRoomInformationRepository roomInformationRepository)
        {
            _roomTypeRepository = roomTypeRepository;
            _roomInformationRepository = roomInformationRepository;
        }

        [BindProperty]
        public RoomInformation RoomInformation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var roominformation = await _roomInformationRepository.GetRoomById(id);

            if (roominformation == null)
            {
                return NotFound();
            }
            else
            {
                RoomInformation = roominformation;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var roominformation = await _roomInformationRepository.GetRoomById(id);

            if (roominformation != null)
            {
                RoomInformation = roominformation;
                var checkDeleteRoom = await _roomInformationRepository.DeleteRoomById(id);
                if (checkDeleteRoom > 0)
                {
                    return RedirectToPage("./Index");
                }
            }
            return NotFound();
        }
    }
}