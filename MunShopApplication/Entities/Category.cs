namespace MunShopApplication.Entities
{
    public class Category
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
    }
}
