namespace ShoeStore.API.Core.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message) : base(message) { }
    }
}
