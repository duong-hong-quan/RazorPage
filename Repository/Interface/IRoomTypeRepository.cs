using DataAccessLayer.Models;

namespace Repository.Interface
{
    public interface IRoomTypeRepository
    {
        Task<ICollection<RoomType>> GetAllRommTypes();
    }
}