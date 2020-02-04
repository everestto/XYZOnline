using XYZOnline.BusinessLogic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XYZOnline.DataAccess
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;

        public OrderService(DataContext context)
        {
            _context = context;
        }

        public async Task<Item> GetReleaseItem(int id)
        {
            return await _context.Items
                            .Where(s => s.Type == ItemType.Release)
                            .Include(s => s.Product)
                                .ThenInclude(p => p.Group)
                            .FirstOrDefaultAsync(s => s.ID == id);
        }

        public async Task<List<Item>> GetReleaseItems()
        {
            return await _context.Items
                            .Where(s => s.Type == ItemType.Release)
                            .Include(s => s.Product)
                                .ThenInclude(p => p.Group)
                            .ToListAsync();
        }
        public async Task<Item> GetOrder(int id)
        {
            return await _context.Items
                            .Where(s => s.Type == ItemType.Order)
                            .Include(s => s.Product)
                                .ThenInclude(p => p.Group)
                            .FirstOrDefaultAsync(s => s.ID == id);
        }

        public async Task<List<Item>> GetOrders()
        {
            return await _context.Items
                            .Where(s => s.Type == ItemType.Order)
                            .Include(s => s.Product)
                                .ThenInclude(p => p.Group)
                            .ToListAsync();
        }

        public string ErrorMessage { get; set; }

        public async Task<bool> ProcessOrder(Item item)
        {
            item.Type = ItemType.Order;
            return await ProcessIventoryItem(item);
        }
        public async Task<bool> ProcessRelease(Item item)
        {
            item.Type = ItemType.Release;
            return await ProcessIventoryItem(item);
        }
        private async Task<bool> ProcessIventoryItem(Item item)
        {
            ErrorMessage = "";
            var service = new InventoryService(_context);
            bool successful = await service.UpdateInventory(item);
            if (successful)
            {
                item.Date = item.Date.Year == 1 ? DateTime.UtcNow : item.Date;
                item.Price = item.Product.Price;

                _context.Items.Add(item);
                await _context.SaveChangesAsync();
            }
            else
            {
                ErrorMessage = service.ErrorMessage;
            }

            return successful;
        }

        public async Task<List<Item>> SearchOrders(string product, string group, ItemType orderType)
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

            return await items.Where(s => s.Type == orderType)
                        .Include(s => s.Product)
                            .ThenInclude(p => p.Group).ToListAsync();
        }

    }
}
