using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XYZOnline.BusinessLogic
{
    public class Product
    {
        public int ID { get; set; }

        [Display(Name="Product Name")]
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(30)]
        public string Description { get; set; }
        public ProductGroup Group { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}
