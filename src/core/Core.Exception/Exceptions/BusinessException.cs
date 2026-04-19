namespace Core.Exception.Exceptions
{
    public class BusinessException : System.Exception
    {
        public BusinessException(string message) : base(message) { }
    }
}
