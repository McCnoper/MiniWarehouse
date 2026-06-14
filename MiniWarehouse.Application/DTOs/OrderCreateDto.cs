namespace MiniWarehouse.Application.DTOs
{
    public class OrderCreateDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public int ProductQuantity { get; set; }
        public int ProductId { get; set; }
    }
}
