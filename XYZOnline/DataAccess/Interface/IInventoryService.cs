using XYZOnline.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XYZOnline.DataAccess
{
    public interface IInventoryService
    {
        Task<Inventory> GetInventory(int id);
        Task<List<Inventory>> GetInventoryByGroup(int id);
        Task<List<Inventory>> GetInventories();
        Task<bool> UpdateInventory(Item item);
        Task<List<Inventory>> SearchInventories(string product, string group);
    }
}
