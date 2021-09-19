using Microsoft.EntityFrameworkCore;
using ShopBridge.Entities;
using ShopBridge.Model;
using ShopBridge.Utility;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.DAL
{
    public class ItemRepository : IItemRepository
    {
        private InventoryDBContext dbContext;
        public ItemRepository(InventoryDBContext inventoryDBContext)
        {
            dbContext = inventoryDBContext;
        }
        public async Task AddItem(Item item)
        {

            DirectoryInfo ImageFolder = new DirectoryInfo(item.ImageFolderPath);

            ItemEntity ItemToBeAdded = new ItemEntity() { Name = item.Name, Brand = item.Brand, Description = item.Description, Price = item.Price, ExpiryDate = item.ExpiryDate };
            ImageEntity imageEntity = new ImageEntity();

            foreach (var file in ImageFolder.GetFiles())
            {
                byte[] bytes;
                bytes = Image.ConvertFileToBytes(file);
                // Validating if file given is a ImageFile
                if (Image.IsItImage(bytes))
                {
                    imageEntity = new ImageEntity() { ImageTitle = file.Name, ImageData = bytes };
                    ItemToBeAdded.Images.Add(imageEntity);
                }
            }
            await this.dbContext.AddAsync(ItemToBeAdded);
            this.dbContext.SaveChanges();
        }
        public async Task<bool> DeleteItem(int id)
        {
            var itemToBeDeleted = await this.dbContext.Items.FindAsync(id);
            if (itemToBeDeleted == null)
            {
                return false;
            }

            this.dbContext.Items.Remove(itemToBeDeleted);
            await this.dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Item>> GetAllItems()
        {
            var AllItems = await this.dbContext.Items.Select(p => new Item
            {
                Name = p.Name,
                Description = p.Description,
                Brand = p.Brand,
                Price = p.Price,
                ExpiryDate = p.ExpiryDate,
                ImageData = p.Images.Select(x=>x.ImageData).ToList()
            }).ToListAsync();
            return AllItems;
        }
        public async Task<bool> UpdateItem(int id, Item i)
        {
            var itemToBeUpdated = await dbContext.Items.FindAsync(id);
            if (itemToBeUpdated == null)
            {
                return false;
            }
            itemToBeUpdated.Name = i.Name;
            itemToBeUpdated.Description = i.Description;
            itemToBeUpdated.Brand = i.Brand;
            itemToBeUpdated.Price = i.Price;
            itemToBeUpdated.ExpiryDate = i.ExpiryDate;
            itemToBeUpdated.Images = i.ImageData?.Select(a => new ImageEntity() { ImageData = a }).ToList();
            try
            {
                await dbContext.SaveChangesAsync();
                return true;
            }

            catch (DbUpdateConcurrencyException e)
            {
                return false;
            }
        }
    }
}
