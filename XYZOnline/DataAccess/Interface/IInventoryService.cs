using XYZOnline.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XYZOnline.DataAccess
{
    public interface IInventoryService
    {
        Inventory GetInventory(int id);
        IEnumerable<Inventory> GetInventories();
        bool UpdateInventory(Item item);
        IEnumerable<Inventory> SearchInventories(string product, string group);
    }
}
