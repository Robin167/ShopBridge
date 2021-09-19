using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using ShopBridge.DAL;
using ShopBridge.Entities;
using ShopBridge.Model;
using ShopBridge.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class ItemRepositoryUnitTest
    {

        private InventoryDBContext context;
        private ItemRepository itemRepository;
        DbContextOptionsBuilder<InventoryDBContext> dbContextOptions;
        public ItemRepositoryUnitTest()
        {

        }

        [SetUp]
        public void Setup()
        {
            dbContextOptions = new DbContextOptionsBuilder<InventoryDBContext>().UseInMemoryDatabase("TestDB");
            context = new InventoryDBContext(dbContextOptions.Options);
            context.Database.EnsureCreated();
            itemRepository = new ItemRepository(context);
        }

        [TearDown]
        public void TearDown()
        {
            itemRepository = null;
            context.Database.EnsureDeleted();
            context = null;
            dbContextOptions = null;
        }

        [Test]
        public async Task AddItem()
        {
            //Act
            await itemRepository.AddItem(JObject.Parse(json).ToObject<Item>());
            byte[] imageData = Image.ConvertFileToBytes(new FileInfo(@".\TestData\abc.jpg"));

            //Assert
            var item = context.Items.Where(x => x.Name == "ABC" && x.Description == "ABC" && x.Brand == "ABC" 
            && x.Images.Select(y=>y.ImageData).Single().SequenceEqual(imageData)
            ).Single();

            Assert.IsNotNull(item);
        }

        [Test]
        public async Task GetItem()
        {
            //Arrange
            var itemToBeAdded = JObject.Parse(json).ToObject<Item>();
            await context.Items.AddAsync(new ItemEntity() {Name = itemToBeAdded.Name,Description=itemToBeAdded.Description,Price=itemToBeAdded.Price,Brand=itemToBeAdded.Brand });
            await context.SaveChangesAsync();

            //Act
            var items = await itemRepository.GetAllItems();

            //Assert
            var item = items.Where(x => x.Name == "ABC" && x.Description == "ABC" && x.Brand == "ABC").Single();
            Assert.IsNotNull(itemToBeAdded);
        }

        [Test]
        public async Task DeleteItem()
        {
            //Arrange
            var itemToBeAdded = JObject.Parse(json).ToObject<Item>();
            await context.Items.AddAsync(new ItemEntity() { Name = itemToBeAdded.Name, Description = itemToBeAdded.Description, Price = itemToBeAdded.Price, Brand = itemToBeAdded.Brand });
            await context.SaveChangesAsync();

            //Act
            var result = await itemRepository.DeleteItem(1);

            //Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateItem()
        {
            //Arrange
            var itemToBeAdded = JObject.Parse(json).ToObject<Item>();
            await context.Items.AddAsync(new ItemEntity() { Name = itemToBeAdded.Name, Description = itemToBeAdded.Description, Price = itemToBeAdded.Price, Brand = itemToBeAdded.Brand });
            await context.SaveChangesAsync();

            //Act
            var result = await itemRepository.UpdateItem(1, JObject.Parse(UpdatedJson).ToObject<Item>());

            //Assert
            Assert.IsTrue(result);
        }

        public string json = @"{
  'Name': 'ABC',
  'Description': 'ABC',
  'Price': 10,
   'Brand':'ABC',
   'ImageFolderPath': '.\\TestData'
}";

        public string UpdatedJson = @"{
  'Name': 'DEF',
  'Description': 'ABC',
  'Price': 10,
   'Brand':'ABC'
}";

    }

}

