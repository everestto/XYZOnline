using XYZOnline.BusinessLogic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XYZOnline.DataAccess
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;

        public OrderService(DataContext context)
        {
            _context = context;
        }

        public Item GetReleaseItem(int id)
        {
            return _context.Items
                            .Where(s => s.Type == ItemType.Release)
                            .Include(s => s.Product)
                                .ThenInclude(p => p.Group)
                            .FirstOrDefault(s => s.ID == id);
        }

        public IEnumerable<Item> GetReleaseItems()
        {
            return _context.Items
                            .Where(s => s.Type == ItemType.Release)
                            .Include(s => s.Product)
                                .ThenInclude(p => p.Group);
        }
        public Item GetOrder(int id)
        {
            return _context.Items
                            .Where(s => s.Type == ItemType.Order)
                            .Include(s => s.Product)
                                .ThenInclude(p => p.Group)
                            .FirstOrDefault(s => s.ID == id);
        }

        public IEnumerable<Item> GetOrders()
        {
            return _context.Items
                            .Where(s => s.Type == ItemType.Order)
                            .Include(s => s.Product)
                                .ThenInclude(p => p.Group);
        }

        public string ErrorMessage { get; set; }

        public bool ProcessOrder(Item item)
        {
            item.Type = ItemType.Order;
            return ProcessIventoryItem( item);
        }
        public bool ProcessRelease(Item item)
        {
            item.Type = ItemType.Release;
            return ProcessIventoryItem(item);
        }
        private bool ProcessIventoryItem(Item item)
        {
            ErrorMessage = "";
            var service = new InventoryService(_context);
            bool successful = service.UpdateInventory(item);
            if (successful)
            {
                item.Date = item.Date.Year == 1 ? DateTime.UtcNow : item.Date;
                item.Price = item.Product.Price;

                _context.Items.Add(item);
                _context.SaveChanges();
            }
            else
            {
                ErrorMessage = service.ErrorMessage;
            }

            return successful;
        }

        public IEnumerable<Item> SearchOrders(string product, string group, ItemType orderType)
        {
            IQueryable<Item> items;

            if (!string.IsNullOrEmpty(product) && !string.IsNullOrEmpty(group))
                items = _context.Items
                        .Where(p => EF.Functions.Like(p.Product.Name, $"%{product}%") && EF.Functions.Like(p.Product.Group.Name, $"%{group}%"));

            else if (!string.IsNullOrEmpty(product))
                items = _context.Items
                        .Where(p => EF.Functions.Like(p.Product.Name, $"%{product}%"));

            else if (!string.IsNullOrEmpty(group))
                items = _context.Items
                        .Where(p => EF.Functions.Like(p.Product.Group.Name, $"%{group}%"));
            else
                items = _context.Items;

            return items.Where(s => s.Type == orderType)
                        .Include(s => s.Product)
                            .ThenInclude(p => p.Group).ToList();
        }

    }
}
