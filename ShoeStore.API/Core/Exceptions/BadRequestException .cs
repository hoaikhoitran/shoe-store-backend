namespace ShoeStore.API.Core.Exceptions
{
    public class BadRequestException : AppException
    {
        public BadRequestException(string message) : base(message) { }
    }
}
