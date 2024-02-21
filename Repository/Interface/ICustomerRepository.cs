using DataAccessLayer.Models;
using RazorPages.Model;

namespace Repository.Interface
{
    public interface ICustomerRepository
    {
        Task<Result<Customer>> Login(string email, string password);

        Task<Result<Customer>> Register(string fullName, string telephone, string email, DateTime? birthday, string status, string password);

        Task<Result<Customer>> UpdateProfile(int customerId, string fullName, string telephone, string email, DateTime? birthday, string status, string password);

        Task<Result<ICollection<Customer>>> GetAllCustomer();

        Task<IList<Customer>> GetAllCustomers();

        Task<Customer> GetCustomerByid(int customerId);

        Task<Customer> UpdateCustomer(int customerId);

        Task<int> DeleteCustomer(int customerId);

        Task<IList<Customer>> Search(string text);
    }
}