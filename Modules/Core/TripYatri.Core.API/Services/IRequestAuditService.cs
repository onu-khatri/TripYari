using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TripYatri.Core.API.Providers
{
    public interface IRequestAuditProvider
    {
        Task Audit(HttpContext httpContext, DateTimeOffset startTime, TimeSpan elapsedTime);
    }
}