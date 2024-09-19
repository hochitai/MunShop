namespace MunShopApplication.Entities
{
    public class User
    {
        public Guid? Id { get; set; } = default(Guid);
        public string? Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreatedAt { get; set; } = null;
        public DateTime? LastUpdatedAt { get; set; } = null;
    }
}
