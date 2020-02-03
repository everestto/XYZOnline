using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace XYZOnline.BusinessLogic
{
    public class Inventory
    {
        public int ID { get; set; }
        public Product Product { get; set; }
        public ItemStatus Status { get; set; }
        public int Quantity { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        [Display(Name = "Total Value")]
        [NotMapped]
        public decimal TotalValue => Quantity * UnitPrice;

    }
}
