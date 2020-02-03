using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;
using XYZOnline.BusinessLogic;
using XYZOnline.Controllers;
using XYZOnline.DataAccess;

namespace XYZOnlineUnitTest
{
    public class ApiUnitTest
    {
        [Fact]
        public void Test1()
        {

        }

        [Fact]
        public void Retrieve_Inventory_Item_Controller_OK_Quantity_Correct()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Retrieve_Inventory_Item_Controller_OK_Quantity_Correct")
                .Options;

            // Run the test against one instance of the context with fresh data
            using (var context = new DataContext(options))
            {
                SeedData.Initialize(context);

                var inventoryService = new InventoryService(context);
                var orderService = new OrderService(context);
                var productService = new ProductService(context);

                var controller = new InventoryController(inventoryService, orderService, productService);

                var result = controller.Get(2); // Inventory Id 2 is HP computer

                Assert.IsType<OkObjectResult>(result);

                var okResult = Assert.IsType<OkObjectResult>(result);
                var item = (Inventory)okResult.Value;

                Assert.Equal(21, item.Quantity);

            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new DataContext(options))
            {
                //Assert.Equal(1, context.Blogs.Count());
                //Assert.Equal("https://example.com", context.Blogs.Single().Url);
            }
        }

        [Fact]
        public void Retrieve_Group_of_Items_Controller_OK_Count_Correct()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "Retrieve_Group_of_Items_Controller_OK_Count_Correct")
                .Options;

            // Run the test against one instance of the context
            using (var context = new DataContext(options))
            {
                SeedData.Initialize(context);

                var inventoryService = new InventoryService(context);
                var orderService = new OrderService(context);
                var productService = new ProductService(context);

                var controller = new InventoryController(inventoryService, orderService, productService);

                var result = controller.GetGroup(2); // 2 is the Product Group ID for Phone.

                Assert.IsType<OkObjectResult>(result);

                var okResult = Assert.IsType<OkObjectResult>(result);
                var item = (Inventory)okResult.Value;

                Assert.Equal(21, item.Quantity);

                //Assert.Equal(12,result.Value);


                var result2 = controller.GetGroup(2); // Group 2 is phone


            }
        }
    }
}