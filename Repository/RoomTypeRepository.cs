using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using Repository.Interface;

namespace Repository
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly IGenericDAO<RoomType> _roomTypeDAO;

        public RoomTypeRepository(IGenericDAO<RoomType> roomTypeDAO)
        {
            _roomTypeDAO = roomTypeDAO;
        }

        public async Task<ICollection<RoomType>> GetAllRommTypes()
        {
            return await _roomTypeDAO.GetAllAsync();
        }
    }
}