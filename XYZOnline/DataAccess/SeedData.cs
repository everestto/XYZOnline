using XYZOnline.BusinessLogic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XYZOnline.DataAccess
{
    public class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var context = new DataContext(serviceProvider.GetRequiredService<DbContextOptions<DataContext>>()))
            {
                // Look for any existing data.
                if (!context.ProductGroups.Any())
                {
                    context.ProductGroups.AddRange(
                        new ProductGroup { Name = "Vehicle" },
                        new ProductGroup { Name = "Phone" },
                        new ProductGroup { Name = "Computer" }
                    );
                }
                context.SaveChanges();

                ProductGroup vehicle = context.ProductGroups.FirstOrDefault(s => s.Name == "Vehicle");
                ProductGroup phone = context.ProductGroups.FirstOrDefault(s => s.Name == "Phone");
                ProductGroup computer = context.ProductGroups.FirstOrDefault(s => s.Name == "Computer");

                // Look for any existing data.
                if (!context.Products.Any())
                {
                    context.Products.AddRange(
                        new Product
                        {
                            // 1
                            Name = "2019 Ford Edge",
                            Description = "2019 Ford Edge Limited SUV",
                            Group = vehicle,
                            Price = 45390M
                        },
                        new Product
                        {
                            // 2
                            Name = "2018 Honda Accord",
                            Description = "2018 Honda Accord Turbo Sedan",
                            Group = vehicle,
                            Price = 23900M
                        },
                        new Product
                        {
                            // 3
                            Name = "Apple iPhone 11 Pro Max",
                            Description = "iPhone 11 Pro Max 256 GB",
                            Group = phone,
                            Price = 1390M
                        },
                        new Product
                        {
                            // 4
                            Name = "HP Spectre",
                            Description = "HP Spectre Laptop Computer",
                            Group = computer,
                            Price = 1190M
                        },
                        new Product
                        {
                            // 5
                            Name = "Samsung Galaxy Note10+",
                            Description = "Samsung Galaxy Note 10 Plus 256 GB",
                            Group = phone,
                            Price = 999M
                        }
                     );
                }
                context.SaveChanges();

                // Look for any existing data.
                if (!context.Items.Any())
                {
                    OrderService orderService = new OrderService(context);

                    await orderService.ProcessOrder(
                        new Item
                        {
                            Product = context.Products.Find(1), // product number 1
                            Quantity = 2,
                            Date = new DateTime(2020, 01, 30)
                        }
                    );
                    await orderService.ProcessOrder(
                        new Item
                        {
                            Product = context.Products.Find(3), // product number 3
                            Quantity = 12,
                            Date = new DateTime(2020, 01, 31)
                        }
                    );
                    await orderService.ProcessOrder(
                        new Item
                        {
                            Product = context.Products.Find(5), // product number 5
                            Quantity = 10,
                            Date = new DateTime(2020, 01, 31)
                        }
                    );
                    await orderService.ProcessOrder(
                        new Item
                        {
                            Product = context.Products.Find(2), // product number 2
                            Quantity = 7,
                            Date = new DateTime(2020, 02, 01)
                        }
                    );
                    await orderService.ProcessOrder(
                        new Item
                        {
                            Product = context.Products.Find(5), // product number 5
                            Quantity = 1,
                            Date = new DateTime(2020, 02, 01)
                        }
                    );
                    await orderService.ProcessOrder(
                        new Item
                        {
                            Product = context.Products.Find(3), // product number 3
                            Quantity = 9,
                        }
                    );
                    await orderService.ProcessOrder(
                        new Item
                        {
                            Product = context.Products.Find(4), // product number 5
                            Quantity = 11,
                            Date = new DateTime(2020, 02, 02)
                        }
                    );
                    await orderService.ProcessOrder(
                        new Item
                        {
                            Product = context.Products.Find(1), // product number 1
                            Quantity = 6,
                            Date = new DateTime(2020, 02, 02)
                        }
                    );

                    // Release
                    await orderService.ProcessRelease(
                        new Item
                        {
                            Product = context.Products.Find(1), // product number 1
                            Quantity = 8,
                            Date = new DateTime(2020, 02, 02)
                        }
                    );
                    await orderService.ProcessRelease(
                        new Item
                        {
                            Product = context.Products.Find(3), // product number 3
                            Quantity = 3,
                        }
                    );

                }
            }
        }
    }
}
