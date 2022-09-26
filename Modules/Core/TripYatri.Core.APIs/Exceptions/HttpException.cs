using System;
using System.Net;

namespace TripYatri.Core.API.Exceptions
{
    public class HttpException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; }

        public HttpException(int httpCode, string message)
            : this((HttpStatusCode)httpCode, message)
        { }

        public HttpException(int httpCode, string message, Exception innerException)
            : this((HttpStatusCode)httpCode, message, innerException)
        { }

        public HttpException(HttpStatusCode httpStatusCode, string message, Exception innerException = null)
            : base(message, innerException)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}