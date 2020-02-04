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
    public class DetailsModel : PageModel
    {
        private readonly IInventoryService _service;
        public DetailsModel(IInventoryService service)
        {
            _service = service;
        }

        public Inventory Inventory { get; set; }

        public async Task OnGetAsync(int id)
        {
            Inventory = await _service.GetInventory(id);
        }
    }
}