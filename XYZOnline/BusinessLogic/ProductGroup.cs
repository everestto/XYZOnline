using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XYZOnline.BusinessLogic
{
    public class ProductGroup
    {
        public int ID { get; set; }

        [Display(Name="Product Group")]
        public string Name { get; set; }

    }
}
