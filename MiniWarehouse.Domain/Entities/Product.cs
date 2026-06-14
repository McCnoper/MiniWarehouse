using System.Collections.Generic;

namespace MiniWarehouse.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string SKU { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; }
        public List<Order> Orders { get; set; } = new();
    }
}
