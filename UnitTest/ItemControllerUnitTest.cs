using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ShopBridge.Controllers;
using ShopBridge.Entities;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopBridge.Model;
using System.Collections.Generic;
using ShopBridge.DAL;
using Moq;
using System.Linq;

namespace UnitTest
{
    public class ItemControllerUnittest
    {
        private ItemController itemController;
        Mock<IItemRepository> mockItemRepository;

        public ItemControllerUnittest()
        {
            
        }

        [SetUp]
        public void Setup()
        {
            mockItemRepository = new Mock<IItemRepository>();
            itemController = new ItemController(mockItemRepository.Object);
        }

        [TearDown]
        public void TearDown()
        {
            itemController = null;
        }

        [Test]
        public async Task AddItem()
        {
            //Arrange
            mockItemRepository.Setup(x => x.AddItem(It.IsAny<Item>()));

            //Act
            var result = await itemController.AddItem(JObject.Parse(json)) as ObjectResult;

            //Assert
            Assert.IsTrue(result.StatusCode == 200);
        }

        [Test]
        public async Task GetItem()
        {
            //Arrange
            IEnumerable<Item> items = new List<Item>() { new Item() {Name="ABC",Description="ABC",Price=10,Brand="ABC" } };
            mockItemRepository.Setup(x => x.GetAllItems()).ReturnsAsync(items);

            //Act
            var AllItems = await itemController.GetAllItems() as OkObjectResult;
            Assert.AreEqual(200, AllItems.StatusCode);

            //Assert
            var ItemsData = AllItems.Value as IEnumerable<JsonData>;
            var itemData = ItemsData.Where(x => x.Name == "ABC"&& x.Price==10 && x.Brand == "ABC" && x.Number == 1).Single();
            Assert.IsNotNull(itemData);
        }
        
        [Test]
        public async Task DeleteItem()
        {
            //Arrange
            mockItemRepository.Setup(x => x.DeleteItem(It.IsAny<int>())).ReturnsAsync(true);

            //Act
            var result = await itemController.DeleteItemUsingQuery(1) as StatusCodeResult;

            //Assert
            Assert.AreEqual(200, result.StatusCode);
        }

        [Test]
        public async Task UpdateItem()
        {
            //Arrange
            IEnumerable<Item> items = new List<Item>() { new Item() { Name = "ABC", Description = "ABC", Price = 10, Brand = "ABC" } };
            mockItemRepository.Setup(x => x.UpdateItem(It.IsAny<int>(), It.IsAny<Item>())).ReturnsAsync(true);

            //Act
            var result = await itemController.UpdateItem(1,new Item()) as StatusCodeResult;

            //Assert
            Assert.AreEqual(200, result.StatusCode);
        }

        public string json = @"{
  'Name': 'ABC',
  'Description': 'ABC',
  'Price': 10,
   'Brand':'ABC'
}";

    }
}