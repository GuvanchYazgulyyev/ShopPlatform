namespace ShopPlatform.Server.Entities
{
    public class Suppliers
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public String Name { get; set; }
        public String WebUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
