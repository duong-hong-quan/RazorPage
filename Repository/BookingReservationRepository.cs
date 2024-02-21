using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using RazorPages.Model;
using Repository.Interface;

namespace Repository
{
    public class BookingReservationRepository : IBookingReservationRepository
    {
        private readonly IGenericDAO<BookingReservation> _bookingDAO;
        private readonly IGenericDAO<Customer> _customerDAO;
        private readonly IGenericDAO<BookingDetail> _bookingDetailDAO;
        private readonly IGenericDAO<RoomInformation> _roomDAO;

        public BookingReservationRepository(IGenericDAO<BookingReservation> bookingDAO, IGenericDAO<Customer> customerDAO,
            IGenericDAO<BookingDetail> bookingDetailDAO, IGenericDAO<RoomInformation> roomDAO)
        {
            _bookingDAO = bookingDAO;
            _customerDAO = customerDAO;
            _bookingDetailDAO = bookingDetailDAO;
            _roomDAO = roomDAO;
        }

        public async Task<Result<ICollection<BookingReservation>>> ViewAllBookingReservationByUserId(int customerId)
        {
            Result<ICollection<BookingReservation>> result = null;
            ICollection<BookingReservation> bookings = await _bookingDAO.GetListByProperty(x => x.CustomerId == customerId);
            if (bookings == null)
            {
                result = new Result<ICollection<BookingReservation>>()
                {
                    Data = null,
                    isSuccess = true,
                    Message = "The customer has not booked anything yet"
                };
                return result;
            }
            else
            {
                result = new Result<ICollection<BookingReservation>>()
                {
                    Data = bookings,
                    isSuccess = true,
                    Message = "The booking reservation of this customer"
                };
                return result;
            }
        }

        public async Task<Result<int>> Booking(int userId, int roomId, DateTime startTime, DateTime endTime)
        {
            Result<int> result = null;
            if (startTime >= endTime)
            {
                result = new Result<int>
                {
                    Data = 0,
                    isSuccess = false,
                    Message = "Can not book with startTime <= endTime"
                };
                return result;
            }
            if (startTime <= DateTime.Now || endTime <= DateTime.Now)
            {
                result = new Result<int>
                {
                    Data = 0,
                    isSuccess = false,
                    Message = "Can not book in the past"
                };
                return result;
            }
            var customer = await _customerDAO.GetByIdAsync(userId);
            var bookingDetail = await _bookingDetailDAO.GetByProperty(x => x.RoomId == roomId);
            var room = await _roomDAO.GetByIdAsync(roomId);

            if (room != null)
            {
                if (bookingDetail == null)
                {
                    var days = (endTime - startTime).Days;
                    var totalPrice = room.RoomPricePerDay * days;
                    var bookingReservationCreated = new BookingReservation()
                    {
                        BookingDate = DateTime.Now,
                        CustomerId = customer.CustomerId,
                        TotalPrice = totalPrice,
                        BookingStatus = 1,
                    };
                    var bookingReservation = await _bookingDAO.AddAsync(bookingReservationCreated);
                    if (bookingReservation != null)
                    {
                        BookingDetail bookingDetailCreated = new BookingDetail()
                        {
                            ActualPrice = totalPrice,
                            EndDate = endTime,
                            StartDate = startTime,
                            RoomId = room.RoomId,
                            BookingReservationId = bookingReservation.BookingReservationId
                        };
                        var CrateBookingDetail = await _bookingDetailDAO.AddAsync(bookingDetailCreated);
                        if (CrateBookingDetail != null)
                        {
                            result = new Result<int>
                            {
                                Data = 1,
                                isSuccess = true,
                                Message = "Created Successfully"
                            };
                            return result;
                        }
                    }
                }
                else
                {
                    if (bookingDetail.StartDate <= startTime && startTime <= bookingDetail.EndDate || bookingDetail.StartDate <= endTime
                        && endTime <= bookingDetail.EndDate || startTime <= bookingDetail.StartDate && endTime >= bookingDetail.EndDate)
                    {
                        result = new Result<int>
                        {
                            Data = 0,
                            isSuccess = false,
                            Message = "Can not create because the room has been booked in that time"
                        };
                        return result;
                    }
                    else
                    {
                        var days = (endTime - startTime).Days;
                        var totalPrice = room.RoomPricePerDay * days;
                        var bookingReservationCreated = new BookingReservation()
                        {
                            BookingDate = DateTime.Now,
                            CustomerId = customer.CustomerId,
                            TotalPrice = totalPrice,
                            BookingStatus = 1,
                        };
                        var bookingReservation = await _bookingDAO.AddAsync(bookingReservationCreated);
                        if (bookingReservation != null)
                        {
                            BookingDetail bookingDetailCreated = new BookingDetail()
                            {
                                ActualPrice = totalPrice,
                                EndDate = endTime,
                                StartDate = startTime,
                                RoomId = room.RoomId,
                                BookingReservationId = bookingReservation.BookingReservationId
                            };
                            var CrateBookingDetail = await _bookingDetailDAO.AddAsync(bookingDetailCreated);
                            if (CrateBookingDetail != null)
                            {
                                result = new Result<int>
                                {
                                    Data = 1,
                                    isSuccess = true,
                                    Message = "Created Successfully"
                                };
                                return result;
                            }
                        }
                    }
                }
            }
            return result = new Result<int>
            {
                Data = 0,
                isSuccess = false,
                Message = "Fail in server"
            };
            return result;
        }

        public async Task<IList<BookingReservation>> GetAll()
        {
            var bookings = await _bookingDAO.GetAllAsyncInclude(x => x.Customer);
            return bookings;
        }

        public async Task<BookingReservation> GetById(int id)
        {
            var booking = await _bookingDAO.GetByIdAsync(id);
            return booking;
        }

        public async Task<int> Deletebooking(int id)
        {
            int check = 0;
            var booking = await _bookingDAO.GetByIdAsync(id);
            var bookingDetail = await _bookingDetailDAO.GetByProperty(x => x.BookingReservationId == booking.BookingReservationId);
            check = await _bookingDetailDAO.DeleteAsync(bookingDetail);
            if (check > 0)
            {
                check = await _bookingDAO.DeleteAsync(booking);
                return check;
            }
            return check;
        }
    }
}