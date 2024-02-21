using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPages.Helper;
using Repository.Interface;

namespace RazorPages.Pages
{
    public class ManageProfileModel : PageModel
    {
        [BindProperty]
        public int CustomerId { get; set; }

        [BindProperty]
        public string CustomerFullName { get; set; }

        [BindProperty]
        public string Telephone { get; set; }

        [BindProperty]
        public string EmailAddress { get; set; }

        [BindProperty]
        public DateTime? CustomerBirthday { get; set; }

        [BindProperty]
        public string CustomerStatus { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ErrorMsg { get; set; }

        public Customer Customer { get; set; }
        private readonly ICustomerRepository _customerRepository;

        public ManageProfileModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public void OnGet()
        {
            Customer = SessionHelper.GetObjectFromJson<Customer>(HttpContext.Session, "Customer");
            if (Customer.CustomerStatus == 1)
            {
                CustomerStatus = "Active";
            }
            else
            {
                CustomerStatus = "Inactive";
            }
            Password = Customer.Password;
            if (Customer.CustomerBirthday == null)
            {
                CustomerBirthday = null;
            }
            else
            {
                CustomerBirthday = Customer.CustomerBirthday;
            }
            EmailAddress = Customer.EmailAddress;
            Telephone = Customer.Telephone;
            CustomerFullName = Customer.CustomerFullName;
            CustomerId = Customer.CustomerId;
        }

        public static IEnumerable<SelectListItem> SelectStatus()
        {
            return new[]
            {
                new SelectListItem {Text = "Active", Value = "1"},
                new SelectListItem {Text = "Inactive", Value = "0"}
            };
        }

        public async Task<IActionResult> OnPost()
        {
            var result = await _customerRepository.UpdateProfile(CustomerId, CustomerFullName, Telephone, EmailAddress, CustomerBirthday,
                CustomerStatus, Password);
            if (!result.isSuccess)
            {
                ErrorMsg = result.Message;
                return Page();
            }
            else
            {
                ErrorMsg = result.Message;
                return Page();
            }
        }
    }
}