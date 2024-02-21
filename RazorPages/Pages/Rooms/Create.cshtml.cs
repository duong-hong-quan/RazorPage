using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Interface;

namespace RazorPages.Pages.Rooms
{
    public class CreateModel : PageModel
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IRoomInformationRepository _roomInformationRepository;

        public CreateModel(IRoomTypeRepository roomTypeRepository, IRoomInformationRepository roomInformationRepository)
        {
            _roomTypeRepository = roomTypeRepository;
            _roomInformationRepository = roomInformationRepository;
        }

        public ICollection<RoomType> RoomTypes { get; set; }

        public async Task<IActionResult> OnGet()
        {
            RoomTypes = await _roomTypeRepository.GetAllRommTypes();
            ViewData["RoomTypeId"] = new SelectList(RoomTypes, "RoomTypeId", "RoomTypeName");
            return Page();
        }

        [BindProperty]
        public RoomInformation RoomInformation { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (RoomInformation == null)
            {
                return Page();
            }

            var room = await _roomInformationRepository.CreateRoom(RoomInformation);

            return RedirectToPage("./Index");
        }
    }
}