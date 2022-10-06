namespace TripYari.Core.Base.Providers.Time
{
    public class TimeProvider : ITimeProvider
    {
        public TimeZoneInfo EstTimeZoneInfo { get; }

        public DateTimeOffset NowInEst => ConvertToEst(DateTimeOffset.UtcNow);

        public DateTimeOffset ConvertToEst(DateTimeOffset dateTimeOffset) =>
            TimeZoneInfo.ConvertTime(dateTimeOffset, EstTimeZoneInfo);

        public DateTimeOffset TodayInEst => DateInEstOf(DateTimeOffset.UtcNow);

        public DateTimeOffset DateInEstOf(DateTimeOffset dateTimeOffset)
        {
            var dateTimeInEst = ConvertToEst(dateTimeOffset);
            return new DateTimeOffset(
                dateTimeInEst.Year,
                dateTimeInEst.Month,
                dateTimeInEst.Day,
                0, 0, 0,
                EstTimeZoneInfo.GetUtcOffset(dateTimeInEst)
            );
        }

        public TimeProvider()
        {
            try
            {
                EstTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
            }
            catch (TimeZoneNotFoundException)
            {
                EstTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            }
        }
    }
}