using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.Helper;

namespace RazorPages.Pages
{
    public class CustomerModel : PageModel
    {
        public Customer Customer { get; set; }

        public void OnGet()
        {
            Customer = SessionHelper.GetObjectFromJson<Customer>(HttpContext.Session, "Customer");
        }
    }
}