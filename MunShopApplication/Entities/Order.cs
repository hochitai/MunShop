namespace MunShopApplication.Entities
{
    public class Order
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }
        public float Total { get; set; }
        public bool IsCanceled { get; set; }
        public List<OrderItem> Items { get; set; } = [];
        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? LastUpdatedAt { get; set; } = null;
    }
}
