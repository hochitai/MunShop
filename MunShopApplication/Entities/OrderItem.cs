namespace MunShopApplication.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid OrderId { get; set; } = Guid.Empty;
        public Guid ProductId { get; set; } = Guid.Empty;
        public float Price { get; set; }
        public int Quantity { get; set; }
        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? LastUpdatedAt { get; set; } = null;
    }
}
