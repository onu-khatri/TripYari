using TripYatri.Core.API.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TripYatri.Core.API.Exceptions;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using TripYatri.Core.Base;
using TripYatri.Core.Base.Providers;
using TripYatri.Core.Base.Exceptions;
using TripYatri.Core.Base.Providers.Logger;

namespace TripYatri.Core.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext, RuntimeEnvironment runtimeContext, ILogger logger)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong: {ex.Message}", ex);
                await HandleExceptionAsync(httpContext, ex, runtimeContext);
            }
        }

        private async  Task HandleExceptionAsync(
            HttpContext context,
            Exception exception,
            RuntimeEnvironment runtimeContext)
        {
            var response = context.Response;
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            if (context.Request.ContentType == "application/xml")
                response.ContentType = "application/xml";
            else
                response.ContentType = "application/json";

            var type = HttpStatusCode.InternalServerError.ToString();

            switch (exception)
            {
                case ArgumentException _:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    type = HttpStatusCode.BadRequest.ToString();
                    break;
                case KeyNotFoundException _:
                case NotFoundException _:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    type = HttpStatusCode.NotFound.ToString();
                    break;
                case Amazon.CloudWatch.Model.LimitExceededException _:
                    response.StatusCode = 429;
                    type = "TooManyRequests";
                    break;
                case TimeoutException _:
                    response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    type = HttpStatusCode.RequestTimeout.ToString();
                    break;
                case UnauthorizedAccessException _:
                case AccessViolationException _:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    type = HttpStatusCode.Unauthorized.ToString();
                    break;
                case HttpException e:
                    response.StatusCode = (int)e.HttpStatusCode;
                    type = e.HttpStatusCode.ToString();
                    break;
            }
            var result="";
            if (context.Request.ContentType == "application/json")
            {

               var error = new Error(
               null,
               context.Response.StatusCode,
               type,
               exception.Message,
               !runtimeContext.IsProduction()
                   ? exception.ToString()
                   : $"{exception.GetType().FullName}: {exception.Message}");

               var apiResponse = new ApiResponse();
               apiResponse.Errors.Add(error);

                 result = JsonConvert.SerializeObject(
                    apiResponse,
                    !runtimeContext.IsProduction()?Newtonsoft.Json.Formatting.Indented: Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        DateFormatHandling = DateFormatHandling.IsoDateFormat,
                        NullValueHandling = NullValueHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
               
            }
            else
            {
                result = GenerateXmlResponse(new Error()
                {
                    Code = context.Response.StatusCode,
                    Type = type,
                    Message = exception.Message,
                    Detail = !runtimeContext.IsProduction() ? exception.ToString() : $"{exception.GetType().FullName}: {exception.Message}"
                }  );  // since XMLSerializer supports parameterless constructer hence  using object initializer way..
                
            }
            await response.WriteAsync(result);

        }
        private string GenerateXmlResponse(Object obj)
        {
            Type t = obj.GetType();
            XmlWriterSettings xmlsettings = new XmlWriterSettings()
            {
                Encoding = System.Text.Encoding.Default
            };

            using (StringWriter stringwriter = new Utf8StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(stringwriter, xmlsettings))
                {
                    var nameSpace = new XmlSerializerNamespaces();
                    nameSpace.Add("", ""); //Empty Namespace
                    XmlSerializer serializer = new XmlSerializer(t);
                    serializer.Serialize(writer, obj, nameSpace);
                    return stringwriter.ToString(); //response
                }
            }
        }
        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}
