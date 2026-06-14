using System.Collections.Generic;
using System.Net.Http.Headers;

namespace MiniWarehouse.Domain.Entities
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = new();
    }
}
