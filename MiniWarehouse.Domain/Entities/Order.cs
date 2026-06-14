using System;

namespace MiniWarehouse.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string CustomerName { get; set; } = string.Empty;
        public int ProductQuantity { get; set; }
        public string Status { get; set; } = "New";
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
