using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XYZOnline.BusinessLogic;
using XYZOnline.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace XYZOnline
{
    public class NewProductModel : PageModel
    {
        private readonly IProductService _service;
        public NewProductModel(IProductService service)
        {
            _service = service;
        }

        [BindProperty]
        public Product Product { get; set; }

        public SelectList ProductGroupOptions { get; set; }

        public IActionResult OnGet()
        {
            PopulateLists();
            return Page();
        }

        public IActionResult OnPost()
        {
            PopulateLists();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool success=_service.Add(Product);
            if (!success)
            {
                ModelState.AddModelError("Product.Name", _service.ErrorMessage);
                return Page();
            }
            return RedirectToPage("Index");
        }

        public void PopulateLists()
        {
            IEnumerable<ProductGroup> groups = _service.GetProductGroups();
            ProductGroupOptions = new SelectList(groups, nameof(ProductGroup.ID), nameof(ProductGroup.Name));
        }

    }
}