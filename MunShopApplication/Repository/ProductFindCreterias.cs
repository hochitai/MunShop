namespace MunShopApplication.Repository
{
    public class ProductFindCreterias : PageCreterias
    {
        public float MinPrice = 0;
        public float MaxPrice = float.MaxValue;
        public string Name = string.Empty;
        public Guid CategoryId = Guid.Empty;
    }
}
