using XYZOnline.BusinessLogic;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace XYZOnline.DataAccess
{
    public class InventoryService : IInventoryService
    {
        private readonly DataContext _context;

        public InventoryService(DataContext context)
        {
            _context = context;
        }

        public Inventory GetInventory(int id)
        {
            return _context.Inventories
                            .Include(s => s.Product)
                                .ThenInclude(p => p.Group)
                            .FirstOrDefault(s=>s.ID==id);
        }
        public IEnumerable<Inventory> GetInventoryByGroup(int id)
        {
            return _context.Inventories
                 .Where(s => s.Product.Group.ID == id)
                 .Include(s => s.Product)
                     .ThenInclude(p => p.Group)
                 .ToList();
        }

        public IEnumerable<Inventory> GetInventories()
        {
            return _context.Inventories
                            .Include(s => s.Product)
                                .ThenInclude(p => p.Group)
                            .ToList();
        }

        public bool UpdateInventory(Item item)
        {
            ErrorMessage = "";

            bool exists = _context.Inventories.Any(s => s.Product == item.Product);

            if (!exists) // New to Inventory
            {
                if (item.Type== ItemType.Release) // Cannot release what does not exist
                {
                    ErrorMessage = "Requested product not available in Inventory";
                    return false;
                }

                // if order then add to the inventory
                Inventory newInventory = new Inventory
                {
                    Product = item.Product,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price,
                    Status = ItemStatus.Instock
                };

                _context.Inventories.Add(newInventory);
                return true;
            }

            // Already existing in inventory
            Inventory inventory = _context.Inventories.FirstOrDefault(s => s.Product == item.Product);

            int sign = 1;

            if(item.Type == ItemType.Release)
            {
                string s = "", are = "is";
                if (inventory.Quantity > 1)
                {
                    are = "are";
                    s = "s"; 
                }
                sign = -1;
                if (inventory.Quantity < item.Quantity)
                {
                    ErrorMessage = $"Insuffient inventory. There {are} only {inventory.Quantity} item{s} in stock";
                    return false;
                }
                if(inventory.Quantity == item.Quantity)
                {
                    inventory.Status = ItemStatus.OutOfStock;
                }
            }
            else
            {
                // Purchase orders should set inventory status to Instock
                inventory.Status = ItemStatus.Instock;
            }

            inventory.Quantity = inventory.Quantity + item.Quantity * sign;
            _context.Inventories.Update(inventory);

            return true;
        }
        public string ErrorMessage { get; set; }
        public IEnumerable<Inventory> SearchInventories(string product, string group)
        {
            IQueryable<Inventory> items;
            if (!string.IsNullOrEmpty(product) && !string.IsNullOrEmpty(group))
                items = _context.Inventories
                        .Where(p => EF.Functions.Like(p.Product.Name, $"%{product}%") && EF.Functions.Like(p.Product.Group.Name, $"%{group}%"));

            else if (!string.IsNullOrEmpty(product))
                items = _context.Inventories
                        .Where(p => EF.Functions.Like(p.Product.Name, $"%{product}%"));

            else if (!string.IsNullOrEmpty(group))
                items = _context.Inventories
                        .Where(p => EF.Functions.Like(p.Product.Group.Name, $"%{group}%"));
            else
                items = _context.Inventories;

            return items.Include(s => s.Product)
                            .ThenInclude(p => p.Group).ToList();
        }
    }
}
