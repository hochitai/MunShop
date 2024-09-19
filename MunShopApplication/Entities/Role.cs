namespace MunShopApplication.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? LastUpdatedAt { get; set; } = null;
    }
}
