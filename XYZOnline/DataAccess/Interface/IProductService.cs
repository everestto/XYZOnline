using XYZOnline.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XYZOnline.DataAccess
{
    public interface IProductService
    {
        Task<Product> GetProduct(int id);
        Task<List<Product>> GetProducts();
        Task<ProductGroup> GetProductGroup(int id);
        Task<List<ProductGroup>> GetProductGroups();
        Task<bool> Add(Product product);
        string ErrorMessage { get; set; }
        Task<List<Product>> SearchProducts(string product, string group);

    }
}
