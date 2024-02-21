using DataAccessLayer.Models;
using RazorPages.Model;

namespace Repository.Interface
{
    public interface IBookingReservationRepository
    {
        Task<Result<ICollection<BookingReservation>>> ViewAllBookingReservationByUserId(int userId);

        Task<Result<int>> Booking(int userId, int roomId, DateTime startTime, DateTime endTime);

        Task<IList<BookingReservation>> GetAll();

        Task<BookingReservation> GetById(int id);

        Task<int> Deletebooking(int id);
    }
}