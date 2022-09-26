using System;
using System.Net;

namespace TripYatri.Core.API.Exceptions
{
    public class NotAcceptableException : HttpException
    {
        public NotAcceptableException(string message, Exception exception = null)
            : base(HttpStatusCode.NotAcceptable, message, exception)
        { }
    }
}
