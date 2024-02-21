using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using Repository.Interface;

namespace Repository
{
    public class RoomInformationRepository : IRoomInformationRepository
    {
        private readonly IGenericDAO<RoomInformation> _roomDAO;
        private readonly IGenericDAO<BookingDetail> _bookingDetailDAO;

        public RoomInformationRepository(IGenericDAO<RoomInformation> roomDAO, IGenericDAO<BookingDetail> bookingDetailDAO)
        {
            _roomDAO = roomDAO;
            _bookingDetailDAO = bookingDetailDAO;
        }

        public async Task<RoomInformation> CreateRoom(RoomInformation Room)
        {
            var room = await _roomDAO.AddAsync(Room);
            return room;
        }

        public async Task<int> DeleteRoomById(int id)
        {
            int roomDeleteCheck = -1;
            var room = await _roomDAO.GetByIdAsync(id);
            var bookingDetail = await _bookingDetailDAO.GetByProperty(x => x.RoomId == room.RoomId);
            if (bookingDetail == null)
            {
                roomDeleteCheck = await _roomDAO.DeleteAsync(room);
                return roomDeleteCheck;
            }
            else
            {
                if (DateTime.Now <= bookingDetail.StartDate || DateTime.Now <= bookingDetail.EndDate && DateTime.Now >= bookingDetail.StartDate)
                {
                    room.RoomStatus = 1;
                    _ = await _roomDAO.UpdateAsync(room);
                    return 1;
                }
                roomDeleteCheck = await _roomDAO.DeleteAsync(room);
                return roomDeleteCheck;
            }
        }

        public async Task<IList<RoomInformation>> GetAllRooms()
        {
            var romms = await _roomDAO.GetAllAsyncInclude(x => x.RoomType);
            return romms;
        }

        public async Task<RoomInformation> GetRoomById(int id)
        {
            var room = await _roomDAO.GetByIdAsync(id);
            return room;
        }

        public async Task<RoomInformation> UpdateRoom(RoomInformation Room)
        {
            var room = await _roomDAO.UpdateAsync(Room);
            return room;
        }

        public async Task<IList<RoomInformation>> Search(string text)
        {
            ICollection<RoomInformation> rooms = await _roomDAO.GetAllAsync();
            var searchString = text.ToLowerInvariant();

            var filteredRooms = rooms
                .Where(room =>
                    room.RoomId.ToString().Contains(searchString) ||
                    room.RoomNumber?.ToLowerInvariant().Contains(searchString) == true ||
                    room.RoomDetailDescription?.ToLowerInvariant().Contains(searchString) == true ||
                    room.RoomMaxCapacity?.ToString().Contains(searchString) == true ||
                    room.RoomTypeId.ToString().Contains(searchString) == true ||
                    room.RoomStatus?.ToString().Contains(searchString) == true ||
                    room.RoomPricePerDay?.ToString().Contains(searchString) == true
                )
                .ToList();

            return filteredRooms;
        }
    }
}