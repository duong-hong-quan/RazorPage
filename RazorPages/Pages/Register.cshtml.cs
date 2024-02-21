using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Interface;

namespace RazorPages.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public string CustomerFullName { get; set; }

        [BindProperty]
        public string Telephone { get; set; }

        [BindProperty]
        public string EmailAddress { get; set; }

        [BindProperty]
        public DateTime CustomerBirthday { get; set; }

        [BindProperty]
        public string CustomerStatus { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ErrorMsg { get; set; }

        private readonly ICustomerRepository _customerRepository;

        public RegisterModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public static IEnumerable<SelectListItem> SelectStatus()
        {
            return new[]
            {
                new SelectListItem {Text = "Active", Value = "1"},
                new SelectListItem {Text = "Inactive", Value = "0"}
            };
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var result = await _customerRepository.Register(CustomerFullName, Telephone, EmailAddress, CustomerBirthday,
                CustomerStatus, Password);

            if (result.isSuccess == true)
            {
                return RedirectToPage("Login");
            }
            else
            {
                ErrorMsg = result.Message;
                return Page();
            }
        }
    }
}