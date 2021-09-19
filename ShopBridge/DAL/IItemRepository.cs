using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopBridge.Model;

namespace ShopBridge.DAL
{
    public interface IItemRepository
    {
        Task AddItem(Item item);
        Task<bool> DeleteItem(int id);
        Task<IEnumerable<Item>> GetAllItems();

        Task<bool> UpdateItem(int id, Item item);
    }
}
