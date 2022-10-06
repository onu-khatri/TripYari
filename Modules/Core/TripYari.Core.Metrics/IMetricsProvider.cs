using System.Runtime.CompilerServices;

namespace TripYari.Core.Metrics
{
    public interface IMetricsProvider : IDisposable
    {
        void Tally(object obj, [CallerMemberName] string methodName = "_", string task = "_", int count = 1);
        void TrackTiming(object obj, TimeSpan elapsedTime, [CallerMemberName] string methodName = "_", string task = "_");
        TimingMetric BeginTiming(object obj, [CallerMemberName] string methodName = "_", string task = "_");
        void FlushMetrics();
    }
}
