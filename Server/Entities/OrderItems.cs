namespace ShopPlatform.Server.Entities
{
    public class OrderItems
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid CreatedUserId { get; set; }
        public Guid OrderId { get; set; }
        public String Description { get; set; }
    }
}
