using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using XYZOnline.BusinessLogic;
using XYZOnline.Controllers;
using XYZOnline.DataAccess;

namespace XYZOnlineUnitTest
{
    public class ApiUnitTest
    {
        [Fact]
        public async System.Threading.Tasks.Task Retrieve_Inventory_Item_Controller_Returns_OK_Quantity_CorrectAsync()
        {
            // Question #4.1
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Retrieve_Inventory_Item_Controller_OK_Quantity_Correct")
                .Options;

            // Run the test against one instance of the context with fresh data
            using (var context = new DataContext(options))
            {
                // Insert data into the Test database
                await TestSeedData.InitializeAsync(context);

                var inventoryService = new InventoryService(context);
                var inventoryController = new InventoryController(inventoryService, null, null);

                // Execute a GET request
                var result = await inventoryController.Get(2); // Inventory Id 2 is HP computer and has 2 entries summing up to quantity of 21 (12+9) 

                //Assert.IsType<OkObjectResult>(result);

                var okResult = Assert.IsType<OkObjectResult>(result);
                var item = (Inventory)okResult.Value;

                Assert.Equal(21, item.Quantity);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task Retrieve_Group_of_Items_Controller_OK_Count_CorrectAsync()
        {
            // Question #4.2
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Retrieve_Group_of_Items_Controller_OK_Count_Correct")
                .Options;

            // Run the test against one instance of the context
            using (var context = new DataContext(options))
            {
                await TestSeedData.InitializeAsync(context);

                var inventoryService = new InventoryService(context);
                var inventoryController = new InventoryController(inventoryService, null, null);

                var result = await inventoryController.GetGroup(2); // 2 is the Product Group ID for Phone. There are 2 phones (Samsung and Apple)
                                                                    // in the inventory therefore should return 2 items

                Assert.IsType<OkObjectResult>(result);

                var okResult = Assert.IsType<OkObjectResult>(result);
                var item = (List<Inventory>)okResult.Value;

                // Inventory contains 
                Assert.Equal(2, item.Count);

                Assert.Equal("Apple iPhone 11 Pro Max", item[0].Product.Name);
                Assert.Equal("Samsung Galaxy Note10+", item[1].Product.Name);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task Increase_Inventory_Count_By_Ordering_and_Quantity_Increased_Async()
        {
            // Question #5.1
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Increase_Inventory_Count_By_Ordering_and_Quantity_Increased")
                .Options;

            // Run the test against one instance of the context
            using (var context = new DataContext(options))
            {
                await TestSeedData.InitializeAsync(context);

                var inventoryService = new InventoryService(context);
                var orderService = new OrderService(context);
                var productService = new ProductService(context);

                var inventoryController = new InventoryController(inventoryService, orderService, productService);

                Item item = new Item
                {
                    Product = new Product { ID = 1, Name = "Ford Edge" }, // Product with ID 1 is Ford Edge. Already has 2 units in inventory from Seed Data. Ordering 20 will push quantity to 22
                    Quantity = 20
                };

                var result = await inventoryController.PostOrder(item);

                Assert.IsType<OkObjectResult>(result);

                // Querying the inventory repository for the inventory position for product Ford Edge
                var inventories = await inventoryService.SearchInventories("Ford", "Vehicle");

                // Inventory contains 22 units of Ford Edge
                Assert.Equal("2019 Ford Edge", inventories[0].Product.Name);
                Assert.Equal(22, inventories[0].Quantity);
            }
        }

        [Fact]
        public async System.Threading.Tasks.Task Decrease_Inventory_Count_By_Releasing_Items_and_Quantity_Decreased_Async()
        {
            // Question #5.2
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Decrease_Inventory_Count_By_Releasing_Items_and_Quantity_Decreased")
                .Options;

            // Run the test against one instance of the context
            using (var context = new DataContext(options))
            {
                await TestSeedData.InitializeAsync(context);

                var inventoryService = new InventoryService(context);
                var orderService = new OrderService(context);
                var productService = new ProductService(context);

                var inventoryController = new InventoryController(inventoryService, orderService, productService);

                Item item = new Item
                {
                    Product = new Product { ID = 4, Name = "Samsung Phone" }, // In seed data, 11 were ordered and 3 released leaving a balance of 8.
                    Quantity = 8 // releasing 8 products now will reduce the inventory balance to 0 and product out of stock.
                };

                var result = await inventoryController.PostRelease(item);

                Assert.IsType<OkObjectResult>(result);

                // Querying the inventory repository for the inventory position for product Ford Edge
                var inventories = await inventoryService.SearchInventories("Samsung", "Phone");

                // Inventory now contains 0 units of Samsung phone
                Assert.Equal("Samsung Galaxy Note10+", inventories[0].Product.Name);
                Assert.Equal(0, inventories[0].Quantity);
                Assert.Equal(ItemStatus.OutOfStock, inventories[0].Status);
            }
        }
    }
}