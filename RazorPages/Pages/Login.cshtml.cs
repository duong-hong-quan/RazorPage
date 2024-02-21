using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.Helper;
using Repository.Interface;

namespace RazorPages.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }
        private readonly IConfiguration _configuration;
        private readonly ICustomerRepository _customerRepository;

        public LoginModel(IConfiguration configuration, ICustomerRepository customerRepository)
        {
            _configuration = configuration;
            _customerRepository = customerRepository;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var AdminEmail = _configuration["AppSettings:AdminEmail"];
            var AdminPassword = _configuration["AppSettings:AdminPassword"];

            // Perform basic validation, authenticate user (dummy example)
            if (AdminEmail == null || AdminPassword == null)
            {
                // Redirect to a protected page upon successful login
                return RedirectToPage("Error");
            }
            else if (Email.Equals(AdminEmail) && Password.Equals(AdminPassword))
            {
                return RedirectToPage("Admin");
            }
            var result = await _customerRepository.Login(Email, Password);
            if (result.Data != null)
            {
                SessionHelper.SetObjectAsJson(HttpContext.Session, "Customer", result.Data);
                return RedirectToPage("Customer");
            }
            // If authentication fails, display an error message
            ErrorMessage = result.Message;
            return Page();
        }
    }
}