using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniWarehouse.Application.DTOs;
using MiniWarehouse.Application.Interfaces;

namespace MiniWarehouse.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IWarehouseService _warehouseService;
        public IndexModel(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }
        public List<ProductDto> Products { get; set; } = new();
        public async Task OnGetAsync()
        {
            Products = await _warehouseService.GetAllProductsWithWarehouseAsync();
        }
    }
}