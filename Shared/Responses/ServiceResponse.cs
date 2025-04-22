namespace ShopPlatform.Shared.Responses
{
    public class ServiceResponse<T> : BaseResponse
    {
        public T Value { get; set; }
    }
}
