namespace MunShopApplication.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid User_id { get; set; }
        public float Total { get; set; }
        public bool IsCanceled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
