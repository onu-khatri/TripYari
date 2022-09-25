using TripYatri.Core.Base.Providers.Metrics;
using System;
using System.Runtime.CompilerServices;

namespace TripYatri.Core.Base.Providers.Metrics
{
    public interface IMetricsProvider : IDisposable
    {
        void Tally(object obj, [CallerMemberName] string methodName = "_", string task = "_", int count = 1);
        void TrackTiming(object obj, TimeSpan elapsedTime, [CallerMemberName] string methodName = "_", string task = "_");
        TimingMetric BeginTiming(object obj, [CallerMemberName] string methodName = "_", string task = "_");
        void FlushMetrics();
    }
}
