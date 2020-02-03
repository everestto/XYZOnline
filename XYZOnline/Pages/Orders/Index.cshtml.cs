using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XYZOnline.BusinessLogic;
using XYZOnline.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace XYZOnline.OrdersPage
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _service;
        public IndexModel(IOrderService service)
        {
            _service = service;
        }

        [BindProperty]
        [Display(Name="Product Name",Prompt ="Product Name")]
        public string ProductName { get; set; }

        [BindProperty]
        [Display(Name = "Product Group", Prompt = "Product Group")]
        public string ProductGroup { get; set; }

        public List<Item> Orders { get; set; }

        public void OnGet()
        {
            Orders = _service.GetOrders().ToList();
        }
        public void OnPost()
        {
            Orders = _service.SearchOrders(ProductName,ProductGroup, ItemType.Order).ToList();
        }
    }
}