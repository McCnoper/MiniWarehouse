using System.Collections.Generic;
using System.Threading.Tasks;
using MiniWarehouse.Application.DTOs;
using MiniWarehouse.Domain.Entities;

namespace MiniWarehouse.Application.Interfaces
{
    public interface IWarehouseService
    {
        Task<List<ProductDto>> GetAllProductsWithWarehouseAsync();
        Task AddProductAsync(Product product);
        Task<Product?> GetProductByIdAsync(int id);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<bool> CreateOrderAsync(OrderCreateDto orderCreateDto);
    }
}
