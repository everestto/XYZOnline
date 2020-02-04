using XYZOnline.BusinessLogic;
using XYZOnline.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<List<Inventory>> Get()
        {
            return await _inventory.GetInventories();
        }

        // GET: api/Inventory/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            var inventory = await _inventory.GetInventory(id);
            if (inventory == null)
            {
                return NotFound();
            }
            return Ok(inventory);
        }

        // GET: api/Inventory/5
        [HttpGet("Group/{id}", Name = "GetGroup")]
        public async Task<IActionResult> GetGroup(int id)
        {

            var inventory = await _inventory.GetInventoryByGroup(id);
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
        public async Task<List<Item>> GetOrders()
        {
            return await _order.GetOrders();
        }

        // GET: api/Inventory/Order/5
        [HttpGet("Order/{id}", Name = "GetOrder")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _order.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        // POST: api/Inventory/Order
        [HttpPost("Order", Name = "PostOrder")]
        public async Task<IActionResult> PostOrder([FromBody] Item item)
        {
            if (item == null)
            {
                return NotFound();
            }

            Product product = await _product.GetProduct(item.Product.ID);
            item.Product = product; // overwrite the product to ensure consistency of ID and name and also prevent inserting new product
            
            bool successful = await _order.ProcessOrder(item);

            if (successful)
                return Ok(item);
            else
                return Ok(_order.ErrorMessage);
        }

        #endregion

        #region Releases

        // GET: api/Inventory/Releases
        [HttpGet("Releases", Name = "GetReleases")]
        public async Task<List<Item>> GetReleases()
        {
            return await _order.GetReleaseItems();
        }

        // GET: api/Inventory/Release/5
        [HttpGet("Release/{id}", Name = "GetRelease")]
        public async Task<IActionResult> GetRelease(int id)
        {
            var order = await _order.GetReleaseItem(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        // POST: api/Inventory/Sale
        [HttpPost("Release", Name = "PostRelease")]
        public async Task<IActionResult> PostRelease([FromBody] Item item)
        {
            if (item == null)
            {
                return NotFound();
            }

            Product product = await _product.GetProduct(item.Product.ID);
            item.Product = product; // overwrite the product to ensure consistency of ID and name and also prevent inserting new product

            bool successful = await _order.ProcessRelease(item);

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
        public async Task<List<Product>> GetProducts()
        {
            return await _product.GetProducts();
        }

        // GET: api/Inventory/5
        [HttpGet("Product/{id}", Name = "GetProduct")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _product.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST: api/Inventory/Product
        [HttpPost("Product", Name = "PostProduct")]
        public async Task<IActionResult> PostProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return NotFound();
            }
            bool successful = await _product.Add(product);

            if (successful)
                return Ok(product);
            else
                return Ok(_order.ErrorMessage);
        }

        #endregion

        #endregion
    }
}
