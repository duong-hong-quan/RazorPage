using DataAccessLayer.Interface;
using DataAccessLayer.Models;
using Microsoft.Extensions.Configuration;
using RazorPages.Model;
using Repository.Interface;
using System.Text.RegularExpressions;

namespace Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IGenericDAO<Customer> _customerDAO;
        private readonly IConfiguration _configuration;

        private string _phonePattern = @"^0\d{9}$";
        private string _emailPattern = @"^.+@.+\..+$";

        public CustomerRepository(IGenericDAO<Customer> customerDAO, IConfiguration configuration)
        {
            _customerDAO = customerDAO;
            _configuration = configuration;
        }

        public async Task<Result<Customer>> Login(string email, string password)
        {
            var user = await _customerDAO.GetByProperty(x => x.EmailAddress.Equals(email));
            Result<Customer> result = null;
            if (user != null)
            {
                if (user.Password.Equals(password) && user.CustomerStatus == 1)
                {
                    result = new Result<Customer>
                    {
                        Data = user,
                        isSuccess = true,
                        Message = "Login with customer successfully"
                    };
                    return result;
                }
                if (user.CustomerStatus != 1)
                {
                    result = new Result<Customer>
                    {
                        Data = null,
                        isSuccess = false,
                        Message = "Customer has been blocked from the web"
                    };
                    return result;
                }
            }
            result = new Result<Customer>
            {
                Data = null,
                isSuccess = false,
                Message = "Invalid Username or password"
            };
            return result;
        }

        private bool CheckValidation(string input, string pattern)
        {
            return Regex.IsMatch(input, pattern);
        }

        public async Task<Result<Customer>> Register(string fullName, string telephone, string email, DateTime? birthday, string status, string password)
        {
            var customer = await _customerDAO.GetByProperty(x => x.EmailAddress.Equals(email));
            if (customer != null)
            {
                Result<Customer> result = new Result<Customer>
                {
                    Data = null,
                    isSuccess = false,
                    Message = "Can not register because this email has been register"
                };
                return result;
            }
            if (birthday > DateTime.Now)
            {
                Result<Customer> result = new Result<Customer>
                {
                    Data = null,
                    isSuccess = false,
                    Message = "Can not register because this birthday is in the future"
                };
                return result;
            }

            if (!CheckValidation(telephone, _phonePattern))
            {
                Result<Customer> result = new Result<Customer>
                {
                    Data = null,
                    isSuccess = false,
                    Message = "Can not register because this phone is not the right pattern"
                };
                return result;
            }

            if (!CheckValidation(email, _emailPattern))
            {
                Result<Customer> result = new Result<Customer>
                {
                    Data = null,
                    isSuccess = false,
                    Message = "Can not register because this email address is not the right pattern"
                };
                return result;
            }

            byte customerStatus = Byte.Parse(status);
            Customer CreatedCustomer = new Customer()
            {
                CustomerStatus = customerStatus,
                CustomerBirthday = birthday,
                EmailAddress = email,
                CustomerFullName = fullName,
                Password = password,
                Telephone = telephone
            };

            var customerCreated = await _customerDAO.AddAsync(CreatedCustomer);

            Result<Customer> response = new Result<Customer>
            {
                Data = customerCreated,
                isSuccess = true,
                Message = "Register Successfully"
            };
            return response;
        }

        public async Task<Result<Customer>> UpdateProfile(int customerId, string fullName, string telephone, string email, DateTime? birthday, string status, string password)
        {
            var customer = await _customerDAO.GetByProperty(x => x.CustomerId == customerId);
            Result<Customer> result = null;
            if (customer != null)
            {
                if (birthday > DateTime.Now)
                {
                    result = new Result<Customer>
                    {
                        Data = null,
                        isSuccess = false,
                        Message = "Can not update because this birthday is in the future"
                    };
                    return result;
                }

                if (!CheckValidation(telephone, _phonePattern))
                {
                    result = new Result<Customer>
                    {
                        Data = null,
                        isSuccess = false,
                        Message = "Can not update because this phone is not the right pattern"
                    };
                    return result;
                }

                if (!CheckValidation(email, _emailPattern))
                {
                    result = new Result<Customer>
                    {
                        Data = null,
                        isSuccess = false,
                        Message = "Can not update because this email address is not the right pattern"
                    };
                    return result;
                }
                byte customerStatus = Byte.Parse(status);
                customer.EmailAddress = email;
                customer.CustomerFullName = fullName;
                customer.CustomerStatus = customerStatus;
                customer.CustomerBirthday = birthday;
                customer.Password = password;
                customer.Telephone = telephone;
                var CustomerUpdated = await _customerDAO.UpdateAsync(customer);
                result = new Result<Customer>
                {
                    Data = CustomerUpdated,
                    isSuccess = true,
                    Message = "Updateed Profile Successfully"
                };
                return result;
            }
            result = new Result<Customer>
            {
                Data = null,
                isSuccess = false,
                Message = "Updateed Profile Fail"
            };
            return result;
        }

        public async Task<Result<ICollection<Customer>>> GetAllCustomer()
        {
            var customers = await _customerDAO.GetAllAsync();
            Result<ICollection<Customer>> result = new Result<ICollection<Customer>>()
            {
                Data = customers,
                isSuccess = true,
                Message = "List of customers"
            };
            return result;
        }

        public async Task<IList<Customer>> GetAllCustomers()
        {
            var customers = await _customerDAO.GetAllAsyncInclude();
            return customers;
        }

        public async Task<Customer> GetCustomerByid(int customerId)
        {
            var customer = await _customerDAO.GetByIdAsync(customerId);
            return customer;
        }

        public Task<Customer> UpdateCustomer(int customerId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteCustomer(int customerId)
        {
            var customer = await _customerDAO.GetByIdAsync(customerId);
            var cusCheck = await _customerDAO.DeleteAsync(customer);
            return cusCheck;
        }

        public async Task<IList<Customer>> Search(string text)
        {
            ICollection<Customer> customers = await _customerDAO.GetAllAsync();
            var searchString = text.ToLowerInvariant();

            var filteredCustomers = customers
                .Where(customer =>
                    customer.CustomerId.ToString().Contains(searchString) == true ||
                    customer.CustomerFullName?.ToLowerInvariant().Contains(searchString) == true ||
                    customer.Telephone?.ToLowerInvariant().Contains(searchString) == true ||
                    customer.EmailAddress.ToLowerInvariant().Contains(searchString) ||
                    customer.CustomerBirthday?.ToString("yyyy-MM-dd").Contains(searchString) == true ||
                    customer.CustomerStatus?.ToString().Contains(searchString) == true
                )
                .ToList();

            return filteredCustomers;
        }
    }
}