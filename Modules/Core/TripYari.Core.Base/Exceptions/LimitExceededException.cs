namespace TripYari.Core.Base.Exceptions
{
    public class LimitExceededException : Exception
    {
        public LimitExceededException()
        {
        }

        public LimitExceededException(string message) : base(message)
        {
        }

        public LimitExceededException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}