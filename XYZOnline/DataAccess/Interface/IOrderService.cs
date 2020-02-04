using XYZOnline.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XYZOnline.DataAccess
{
    public interface IOrderService
    {
        Task<Item> GetReleaseItem(int id);
        Task<List<Item>> GetReleaseItems();
        Task<Item> GetOrder(int id);
        Task<List<Item>> GetOrders();
        Task<List<Item>> SearchOrders(string product,string group, ItemType itemType);
        string ErrorMessage { get; set; }
        Task<bool> ProcessOrder(Item item);
        Task<bool> ProcessRelease(Item item);

    }
}
