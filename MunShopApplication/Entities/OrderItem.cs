namespace MunShopApplication.Entities
{
    public class OrderItem
    {
        public Guid? Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid? ProductId { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? LastUpdatedAt { get; set; } = null;
    }
}
