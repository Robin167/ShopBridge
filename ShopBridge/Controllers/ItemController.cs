using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ShopBridge.Entities;
using ShopBridge.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private InventoryDBContext dbContext;
        public ItemController(InventoryDBContext inventoryDBContext)
        {
            dbContext = inventoryDBContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] JObject i)
        {
            try
            {
                var item = i?.ToObject<Item>();
                if (item.Price < 0)
                {
                    return BadRequest();
                }

                ItemEntity ItemToBeAdded = new ItemEntity() { Name = item.Name, Brand = item.Brand, Description = item.Description, Price = item.Price, ExpiryDate = item.ExpiryDate };
                await this.dbContext.AddAsync(ItemToBeAdded);
                this.dbContext.SaveChanges();
                return Ok("Sucessfully Added");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetAllItems()
        {
            var AllItems =  await this.dbContext.Items.Select(p => new Item
            {
                Name = p.Name,
                Description = p.Description,
                Brand = p.Brand,
                Price = p.Price,
                ExpiryDate = p.ExpiryDate
            }).ToListAsync();

            if(AllItems.Count == 0)
            {
                return NotFound();
            }

            return Ok(AllItems);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteItemUsingQuery([FromQuery] int id)
        {
            var itemToBeDeleted = await this.dbContext.Items.FindAsync(id);
            if (itemToBeDeleted == null)
            {
                return NotFound();
            }

            this.dbContext.Items.Remove(itemToBeDeleted);
            await this.dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemUsingURL(int id)
        {
            var itemToBeDeleted = await this.dbContext.Items.FindAsync(id);
            if (itemToBeDeleted == null)
            {
                return NotFound();
            }

            this.dbContext.Items.Remove(itemToBeDeleted);
            await this.dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItem(int id,[FromBody]Item i)
        {
            var itemToBeUpdated = await dbContext.Items.FindAsync(id);
            if(itemToBeUpdated == null)
            {
                return NotFound();
            }

            itemToBeUpdated.Name = i.Name;
            itemToBeUpdated.Description = i.Description;
            itemToBeUpdated.Brand = i.Brand;
            itemToBeUpdated.Price = i.Price;
            itemToBeUpdated.ExpiryDate = i.ExpiryDate;
            try
            {
                await dbContext.SaveChangesAsync();
            }

            catch(DbUpdateConcurrencyException e)
            {
                
            }
            
            return Ok();
        }
    }
}
