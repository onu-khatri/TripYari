namespace TripYari.Core.Base.Providers.Time
{
    public interface ITimeProvider
    {
        TimeZoneInfo EstTimeZoneInfo { get; }
        DateTimeOffset NowInEst { get; }
        DateTimeOffset ConvertToEst(DateTimeOffset dateTimeOffset);
        DateTimeOffset TodayInEst { get; }
        DateTimeOffset DateInEstOf(DateTimeOffset dateTimeOffset);
    }   
}