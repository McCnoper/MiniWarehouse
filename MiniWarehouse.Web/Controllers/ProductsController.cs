using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using MiniWarehouse.Application.DTOs;
using MiniWarehouse.Application.Interfaces;
using MiniWarehouse.Domain.Entities;

namespace MiniWarehouse.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;
        public ProductsController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }
        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetProducts()
        {
            var products = await _warehouseService.GetAllProductsWithWarehouseAsync();
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _warehouseService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);

        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _warehouseService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        [HttpPost("order")]
        public async Task<ActionResult> CreateOrder([FromBody] OrderCreateDto orderDto)
        {
            var success = await _warehouseService.CreateOrderAsync(orderDto);
            if (!success)
            {
                return BadRequest("Недостатньо товару на складі або товар не знайдено.");
            }
            return Ok("Замовлення успішно створено.");
        }
    }
}