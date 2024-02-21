using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Interface;

namespace RazorPages.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [BindProperty(SupportsGet = true)]
        public string ErrorMsg { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            string status = Customer.CustomerStatus.ToString();
            var customer = await _customerRepository.Register(Customer.CustomerFullName, Customer.Telephone, Customer.EmailAddress, Customer.CustomerBirthday, status, Customer.Password);
            if (!customer.isSuccess)
            {
                ErrorMsg = customer.Message;
                return Page();
            }
            return RedirectToPage("./Index");
        }

        public static IEnumerable<SelectListItem> SelectStatus()
        {
            return new[]
            {
                new SelectListItem {Text = "Active", Value = "1"},
                new SelectListItem {Text = "Inactive", Value = "0"}
            };
        }
    }
}