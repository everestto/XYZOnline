using XYZOnline.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XYZOnline.DataAccess
{
    public interface IOrderService
    {
        Item GetReleaseItem(int id);
        IEnumerable<Item> GetReleaseItems();
        Item GetOrder(int id);
        IEnumerable<Item> GetOrders();
        IEnumerable<Item> SearchOrders(string product,string group, ItemType itemType);
        string ErrorMessage { get; set; }
        bool ProcessOrder(Item item);
        bool ProcessRelease(Item item);

    }
}
