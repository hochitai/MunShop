namespace MunShopApplication.Entities
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string Image { get; set; } = string.Empty;

        public Guid CategoryId  { get; set; }
        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? LastUpdatedAt { get; set; } = null;
    }
}
