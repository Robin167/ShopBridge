using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ShopBridge.DAL;
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
        private IItemRepository ItemRepository;
        public ItemController(IItemRepository itemRepository)
        {
            this.ItemRepository = itemRepository;
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

                await ItemRepository.AddItem(item);
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
            int count = 1;
            var AllItems = await ItemRepository.GetAllItems();
            if(AllItems == null)
            {
                return NotFound();
            }
            
            return Ok(AllItems.Select(a => new JsonData()
            {
                Number = count++,
                Name = a.Name, Brand = a.Brand,Description = a.Description,Price = a.Price,Images = a.ImageData
            }));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteItemUsingQuery([FromQuery] int id)
        {
            bool result = await this.ItemRepository.DeleteItem(id);
            if (result == false)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemUsingURL(int id)
        {
            bool result = await this.ItemRepository.DeleteItem(id);
            if (result == false)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItem(int id,[FromBody]Item i)
        {
            var result = await this.ItemRepository.UpdateItem(id, i);
            if(result == false)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}

