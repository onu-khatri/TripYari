using Newtonsoft.Json;
using System.Collections.Generic;

namespace TripYatri.Core.API.Models
{
    public class ApiResponse
    {
        [JsonProperty("errors")] public List<Error> Errors { get; set; } = new List<Error>();

        [JsonProperty("forensics")] public dynamic Forensics { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public ApiResponse(T data)
        {
            Data = data;
        }

        [JsonProperty("data")] public T Data { get; set; }
    }
}