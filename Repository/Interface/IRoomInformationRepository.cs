using DataAccessLayer.Models;

namespace Repository.Interface
{
    public interface IRoomInformationRepository
    {
        Task<IList<RoomInformation>> GetAllRooms();

        Task<RoomInformation> CreateRoom(RoomInformation room);

        Task<RoomInformation> UpdateRoom(RoomInformation room);

        Task<RoomInformation> GetRoomById(int id);

        Task<int> DeleteRoomById(int id);

        Task<IList<RoomInformation>> Search(string text);
    }
}