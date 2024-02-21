using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.Interface;

namespace RazorPages.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly ICustomerRepository _customerRepository;

        public EditModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [BindProperty(SupportsGet = true)]
        public string ErrorMsg { get; set; }

        [BindProperty]
        public Customer Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerByid(id);
            if (customer == null)
            {
                return NotFound();
            }
            Customer = customer;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            string status = Customer.CustomerStatus.ToString();
            var result = await _customerRepository.UpdateProfile(Customer.CustomerId, Customer.CustomerFullName, Customer.Telephone, Customer.EmailAddress, Customer.CustomerBirthday,
               status, Customer.Password);
            if (!result.isSuccess)
            {
                ErrorMsg = result.Message;
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