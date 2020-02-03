using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XYZOnline.BusinessLogic
{
    public class Item
    {
        public int ID { get; set; }
        public Product Product { get; set; }
        public ItemType Type { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int Quantity { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        [Range(0D, double.MaxValue, ErrorMessage = "Only positive number allowed")]
        public decimal Price { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        [Display(Name ="Total Value")]
        [NotMapped]
        public decimal TotalValue => Quantity * Price;

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Date { get; set; }
    }
}
