using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Interface;

namespace RazorPages.Pages.Students
{
    public class DeleteModel : PageModel
    {
        private readonly ICustomerRepository _customerRepository;

        public DeleteModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerByid(id);

            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                Customer = customer;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var customerCheck = await _customerRepository.DeleteCustomer(id);
            if (customerCheck > 0)
            {
                return RedirectToPage("./Index");
            }
            return NotFound();
        }
    }
}