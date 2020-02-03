using XYZOnline.BusinessLogic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XYZOnline.DataAccess
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
       : base(options)
        {
        }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Inventory> Inventories { get; set; }


    }
}
