namespace MunShopApplication.DTOs
{
    public class UserResponse
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? RoleId { get; set; } 
        public string? Token { get; set; }    
    }
}
