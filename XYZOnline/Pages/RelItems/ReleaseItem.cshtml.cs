using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XYZOnline.BusinessLogic;
using XYZOnline.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace XYZOnline.SalesPage
{
    public class ReleaseItemModel : PageModel
    {
        private readonly IOrderService _service;
        private readonly IProductService _product;

        public ReleaseItemModel(IOrderService service,IProductService product)
        {
            _service = service;
            _product = product;
        }

        public class ReleaseItem
        {
            [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
            public int Quantity { get; set; }

            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
            [Range(0D, double.MaxValue, ErrorMessage = "Only positive number allowed")]
            public decimal UnitPrice { get; set; }
        }

        [Display(Name = "Product")]
        [BindProperty(SupportsGet = true)]
        public int ProductID { get; set; }

        [BindProperty]
        public ReleaseItem Release { get; set; }

        public SelectList ProductListOptions { get; set; }

        //[BindProperty]
        //public int ProdType { get; set; }

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

            Product product = _product.GetProduct(ProductID);
            Item releaseItem = new Item
            {
                Product = product,
                Quantity = Release.Quantity,
                Price = Release.UnitPrice,
            };

           bool success= _service.ProcessRelease(releaseItem);
            if (!success)
            {                
                ModelState.AddModelError("", _service.ErrorMessage);
                return Page();
            }

            return RedirectToPage("Index");
        }

        public void PopulateLists()
        {
            var products = _product.GetProducts();
            ProductListOptions = new SelectList(products, nameof(Product.ID), nameof(Product.Name));
        }

        public JsonResult OnGetUnitPrice()
        {
            var json=new JsonResult(_product.GetProduct(ProductID).Price);
            return json;
        }
    }
}