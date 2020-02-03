using XYZOnline.BusinessLogic;
using XYZOnline.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace XYZOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventory;
        private readonly IOrderService _order;
        private readonly IProductService _product;

        public InventoryController(IInventoryService inventory,IOrderService order,IProductService product)
        {
            _inventory = inventory;
            _order = order;
            _product = product;
        }

        #region Inventory

        // GET: api/Inventory
        [HttpGet]
        public IEnumerable<Inventory> Get()
        {
            return _inventory.GetInventories();
        }

        // GET: api/Inventory/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var inventory = _inventory.GetInventory(id);
            if (inventory == null)
            {
                return NotFound();
            }
            return Ok(inventory);
        }

        #endregion

        #region Orders

        // GET: api/Inventory/Orders
        [HttpGet("Orders", Name = "GetOrders")]
        public IEnumerable<Item> GetOrders()
        {
            return _order.GetOrders();
        }

        // GET: api/Inventory/Order/5
        [HttpGet("Order/{id}", Name = "GetOrder")]
        public IActionResult GetOrder(int id)
        {
            var order = _order.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        // POST: api/Inventory/Order
        [HttpPost("Order", Name = "PostOrder")]
        public IActionResult PostOrder([FromBody] Item item)
        {
            if (item == null)
            {
                return NotFound();
            }

            Product product = _product.GetProduct(item.Product.ID);
            item.Product = product; // overwrite the product to ensure consistency of ID and name and also prevent inserting new product
            
            bool successful = _order.ProcessOrder(item);

            if (successful)
                return Ok(item);
            else
                return Ok(_order.ErrorMessage);
        }

        #endregion

        #region Sales

        // GET: api/Inventory/Releases
        [HttpGet("Releases", Name = "GetReleases")]
        public IEnumerable<Item> GetReleases()
        {
            return _order.GetReleaseItems();
        }

        // GET: api/Inventory/Release/5
        [HttpGet("Release/{id}", Name = "GetRelease")]
        public IActionResult GetRelease(int id)
        {
            var order = _order.GetReleaseItem(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        // POST: api/Inventory/Sale
        [HttpPost("Release", Name = "PostRelease")]
        public IActionResult PostRelease([FromBody] Item item)
        {
            if (item == null)
            {
                return NotFound();
            }

            Product product = _product.GetProduct(item.Product.ID);
            item.Product = product; // overwrite the product to ensure consistency of ID and name and also prevent inserting new product

            bool successful = _order.ProcessRelease(item);

            if (successful)
                return Ok(item);
            else
                return Ok(_order.ErrorMessage);
        }

        #endregion

        #region Extras

        #region Products

        // GET: api/Inventory/Products
        [HttpGet("Products", Name = "GetProducts")]
        public IEnumerable<Product> GetProducts()
        {
            return _product.GetProducts();
        }

        // GET: api/Inventory/5
        [HttpGet("Product/{id}", Name = "GetProduct")]
        public IActionResult GetProduct(int id)
        {
            var product = _product.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST: api/Inventory/Product
        [HttpPost("Product", Name = "PostProduct")]
        public IActionResult PostProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return NotFound();
            }
            bool successful = _product.Add(product);

            if (successful)
                return Ok(product);
            else
                return Ok(_order.ErrorMessage);
        }

        #endregion

        #endregion
    }
}
