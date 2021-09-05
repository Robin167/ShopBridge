using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ShopBridge.Controllers;
using ShopBridge.Entities;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopBridge.Model;
using System.Collections.Generic;


namespace UnitTest
{
    public class ItemControllerUnittest
    {
        private InventoryDBContext context;
        private ItemController itemController;
        DbContextOptionsBuilder<InventoryDBContext> dbContextOptions;
        public ItemControllerUnittest()
        {
            
        }

        [SetUp]
        public void Setup()
        {
            dbContextOptions = new DbContextOptionsBuilder<InventoryDBContext>().UseInMemoryDatabase("TestDB");
            context = new InventoryDBContext(dbContextOptions.Options);
            context.Database.EnsureCreated();
            itemController = new ItemController(context);
        }

        [TearDown]
        public void TearDown()
        {
            itemController = null;
            context.Database.EnsureDeleted();
            context = null;
            dbContextOptions = null;
        }

        [Test]
        public async Task AddItem()
        {
            var result = await itemController.AddItem(JObject.Parse(json)) as ObjectResult;
            var a = result.StatusCode;
            Assert.IsTrue(a == 200);
        }

        [Test]
        public async Task GetItem()
        {
            await itemController.AddItem(JObject.Parse(json));
            var AllItems = await itemController.GetAllItems() as ObjectResult;
            var v = AllItems.Value as List<Item>;
            Assert.AreEqual(200, AllItems.StatusCode);
            Assert.IsTrue(v.Count == 1);
            Assert.IsTrue(v[0].Name == "ABC");
            Assert.IsTrue(v[0].Description == "ABC");
            Assert.IsTrue(v[0].Price == 10);
            Assert.IsTrue(v[0].Brand == "ABC");
        }

        [Test]
        public async Task DeleteItem()
        {
            await itemController.AddItem(JObject.Parse(json));
            var AllItems = await itemController.GetAllItems() as ObjectResult;
            var v = AllItems.Value as List<Item>;
            Assert.AreEqual(200, AllItems.StatusCode);
            Assert.IsTrue(v.Count == 1);

            await itemController.DeleteItemUsingURL(1);
            AllItems = await itemController.GetAllItems() as ObjectResult;
            Assert.IsNull(AllItems);
        }

        [Test]
        public async Task UpdateItem()
        {
            await itemController.AddItem(JObject.Parse(json));
            var AllItems = await itemController.GetAllItems() as ObjectResult;
            var v = AllItems.Value as List<Item>;
            
            Assert.AreEqual(200, AllItems.StatusCode);
            Assert.IsTrue(v.Count == 1);

            await itemController.UpdateItem(1,JObject.Parse(UpdatedJson).ToObject<Item>());
            AllItems = await itemController.GetAllItems() as ObjectResult;
            v = AllItems.Value as List<Item>;
            Assert.AreEqual(200, AllItems.StatusCode);
            Assert.IsTrue(v[0].Name == "DEF");
        }

        public string json = @"{
  'Name': 'ABC',
  'Description': 'ABC',
  'Price': 10,
   'Brand':'ABC'
}";

        public string UpdatedJson = @"{
  'Name': 'DEF',
  'Description': 'ABC',
  'Price': 10,
   'Brand':'ABC'
}";

    }
}