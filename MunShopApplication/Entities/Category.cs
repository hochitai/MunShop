namespace MunShopApplication.Entities
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? LastUpdatedAt { get; set; } = null;
    }
}
