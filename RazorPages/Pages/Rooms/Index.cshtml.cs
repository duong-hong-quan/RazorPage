using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Interface;

namespace RazorPages.Pages.Rooms
{
    public class IndexModel : PageModel
    {
        private readonly IRoomInformationRepository _roomInformationRepository;

        public IndexModel(IRoomInformationRepository roomInformationRepository)
        {
            _roomInformationRepository = roomInformationRepository;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public IList<RoomInformation> RoomInformation { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (!string.IsNullOrEmpty(SearchString))
            {
                RoomInformation = await _roomInformationRepository.Search(SearchString);
            }
            else
            {
                RoomInformation = await _roomInformationRepository.GetAllRooms();
            }
            return Page();
        }

        public static IEnumerable<SelectListItem> SelectStatus()
        {
            return new[]
            {
                new SelectListItem {Text = "Active", Value = "1"},
                new SelectListItem {Text = "Inactive", Value = "0"}
            };
        }
    }
}