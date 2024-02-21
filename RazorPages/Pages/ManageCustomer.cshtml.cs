using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.Interface;

namespace RazorPages.Pages
{
    public class ManageCustomerModel : PageModel
    {
        public ICollection<Customer> Customers { get; set; }
        private readonly ICustomerRepository _customerRepository;

        public ManageCustomerModel(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IActionResult> OnGet()
        {
            var result = await _customerRepository.GetAllCustomer();
            Customers = result.Data;
            return Page();
        }
    }
}