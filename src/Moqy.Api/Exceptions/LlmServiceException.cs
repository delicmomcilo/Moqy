namespace Moqy.Api.Exceptions
{
    public class LlmServiceException : Exception
    {
        public LlmServiceException(string message) : base(message)
        {
        }

        public LlmServiceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}