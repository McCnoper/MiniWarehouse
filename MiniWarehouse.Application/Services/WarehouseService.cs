using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MiniWarehouse.Application.Interfaces;
using MiniWarehouse.Application.DTOs;
using MiniWarehouse.Domain.Entities;
using MiniWarehouse.Infrastructure.Data;

namespace MiniWarehouse.Application.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly WarehouseDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<WarehouseService> _logger;

        public WarehouseService(WarehouseDbContext context, IMapper mapper, IMemoryCache cache, ILogger<WarehouseService> logger)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }

        public async Task<List<ProductDto>> GetAllProductsWithWarehouseAsync()
        {
            string cacheKey = "productsListKey";


            if (!_cache.TryGetValue(cacheKey, out List<ProductDto>? productsDto))
            {
 
                _logger.LogWarning("Даних немає в кеші. Виконуємо запит до БД...");

                var products = await _context.Products
                    .Include(p => p.Warehouse)
                    .OrderBy(p => p.Name)
                    .ToListAsync();

                productsDto = _mapper.Map<List<ProductDto>>(products);

      
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

                _cache.Set(cacheKey, productsDto, cacheOptions);
                _logger.LogInformation("Дані успішно завантажено з БД і збережено в кеш.");
            }
            else
            {
                _logger.LogInformation("ОТРИМАНО З КЕШУ! Запит до БД не виконувався.");
            }

            return productsDto ?? new List<ProductDto>();
        }

        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Увага! Додано новий товар: {product.Name}");

          
            _cache.Remove("productsListKey");
        }

        public async Task<Product?> GetProductByIdAsync(int id) => await _context.Products.FindAsync(id);

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            _cache.Remove("productsListKey");
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                _cache.Remove("productsListKey");
            }
        }

        public async Task<bool> CreateOrderAsync(OrderCreateDto orderDto)
        {
            var product = await _context.Products.FindAsync(orderDto.ProductId);

            if (product == null || product.Quantity < orderDto.ProductQuantity)
            {
                _logger.LogError($"Помилка створення замовлення. Товар ID {orderDto.ProductId} не знайдено або недостатньо на складі.");
                return false;
            }

            product.Quantity -= orderDto.ProductQuantity;

            var order = _mapper.Map<Order>(orderDto);
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Створено замовлення на {orderDto.ProductQuantity} шт. товару {product.Name}");
            _cache.Remove("productsListKey");

            return true;
        }
    }
}