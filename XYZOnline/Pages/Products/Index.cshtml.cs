using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XYZOnline.BusinessLogic;
using XYZOnline.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace XYZOnline.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _service;

        public IndexModel(IProductService service)
        {
            _service = service;
        }

        [BindProperty]
        [Display(Name = "Product Name", Prompt = "Product Name")]
        public string ProductName { get; set; }

        [BindProperty]
        [Display(Name = "Product Group", Prompt = "Product Group")]
        public string ProductGroup { get; set; }

        public List<Product> Products { get; set; }

        public async Task OnGetAsync()
        {
            Products = await _service.GetProducts();
        }
        public async Task OnPostAsync()
        {
            Products = await _service.SearchProducts(ProductName, ProductGroup);
        }
    }
}