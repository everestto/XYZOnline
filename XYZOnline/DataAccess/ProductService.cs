using XYZOnline.BusinessLogic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XYZOnline.DataAccess
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public Product GetProduct(int id)
        {
            return _context.Products.Include(p => p.Group).FirstOrDefault(s => s.ID == id);
        }
        public IEnumerable<Product> GetProducts()
        {
            return _context.Products.Include(p => p.Group);
        }

        public ProductGroup GetProductGroup(int id)
        {
            return _context.ProductGroups.Find(id);
        }
        public IEnumerable<ProductGroup> GetProductGroups()
        {
            return _context.ProductGroups;
        }

        public string ErrorMessage { get; set; }
        public bool Add(Product product)
        {
            bool success = false;
            try
            {
                if (!_context.Products.Any(s => s.Name.Equals(product.Name)))
                {
                    product.Group = _context.ProductGroups.Find(product.Group.ID);
                    _context.Products.Add(product);
                    _context.SaveChanges();
                    success = true;
                }
                else
                {
                    ErrorMessage = "Product already exists";
                }
            }catch(Exception ex)
            {
                string mes = ex.Message;
                ErrorMessage = "Error ocurred";
            }
            return success;
        }

        public IEnumerable<Product> SearchProducts(string product, string group)
        {
            IQueryable<Product> items;
            if (!string.IsNullOrEmpty(product) && !string.IsNullOrEmpty(group))
                items = _context.Products
                        .Where(p => EF.Functions.Like(p.Name, $"%{product}%") && EF.Functions.Like(p.Group.Name, $"%{group}%"));

            else if (!string.IsNullOrEmpty(product))
                items = _context.Products
                        .Where(p => EF.Functions.Like(p.Name, $"%{product}%"));

            else if (!string.IsNullOrEmpty(group))
                items = _context.Products
                        .Where(p => EF.Functions.Like(p.Group.Name, $"%{group}%"));
            else
                items = _context.Products;

            return items.Include(s => s.Group).ToList();
        }

    }
}
