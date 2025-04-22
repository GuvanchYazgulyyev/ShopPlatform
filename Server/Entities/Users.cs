namespace ShopPlatform.Server.Entities
{
    public class Users
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String EMailAddress { get; set; }
        public bool IsActive { get; set; }
        public String Password { get; set; }
    }
}
