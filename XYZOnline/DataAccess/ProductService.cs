using XYZOnline.BusinessLogic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XYZOnline.DataAccess
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProduct(int id)
        {
            return await _context.Products.Include(p => p.Group).FirstOrDefaultAsync(s => s.ID == id);
        }
        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products.Include(p => p.Group).ToListAsync();
        }

        public async Task<ProductGroup> GetProductGroup(int id)
        {
            return await _context.ProductGroups.FindAsync(id);
        }
        public async Task<List<ProductGroup>> GetProductGroups()
        {
            return await _context.ProductGroups.ToListAsync();
        }

        public string ErrorMessage { get; set; }
        public async Task<bool> Add(Product product)
        {
            bool success = false;
            try
            {
                if (!await _context.Products.AnyAsync(s => s.Name.Equals(product.Name)))
                {
                    product.Group = await _context.ProductGroups.FindAsync(product.Group.ID);
                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();
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

        public async Task<List<Product>> SearchProducts(string product, string group)
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

            return await items.Include(s => s.Group).ToListAsync();
        }

    }
}
