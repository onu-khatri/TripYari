namespace TripYatri.Core.API.Models
{
    public class Error
    {
        public Error() { }
        public Error(string propertyName, int? code, string type, string message,string detail)
        {
            PropertyName = propertyName;
            Code = code;
            Type = type;
            Message = message;
            Detail = detail;
        }
        public string PropertyName { get; set; }
        public int? Code { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
    }
}
