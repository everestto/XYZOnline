using XYZOnline.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XYZOnline.DataAccess
{
    public interface IProductService
    {
        public Product GetProduct(int id);
        public IEnumerable<Product> GetProducts();
        public ProductGroup GetProductGroup(int id);
        public IEnumerable<ProductGroup> GetProductGroups();
        bool Add(Product product);
        public string ErrorMessage { get; set; }
        public IEnumerable<Product> SearchProducts(string product, string group);

    }
}
