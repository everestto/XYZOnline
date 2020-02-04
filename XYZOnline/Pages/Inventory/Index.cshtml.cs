using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XYZOnline.BusinessLogic;
using XYZOnline.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace XYZOnline.InventoryPage
{
    public class IndexModel : PageModel
    {
        private readonly IInventoryService _service;
        public IndexModel(IInventoryService service)
        {
            _service = service;
        }

        [BindProperty]
        [Display(Name="Product Name",Prompt ="Product Name")]
        public string ProductName { get; set; }

        [BindProperty]
        [Display(Name = "Product Group", Prompt = "Product Group")]
        public string ProductGroup { get; set; }

        public List<Inventory> Inventories { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Inventories = await _service.GetInventories();
            return Page();
        }

        public async Task OnPostAsync()
        {
            Inventories = await _service.SearchInventories(ProductName, ProductGroup);
        }

    }
}