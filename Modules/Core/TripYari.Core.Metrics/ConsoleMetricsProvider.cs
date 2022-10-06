using System.Runtime.CompilerServices;
using TripYari.Core.Loggers;

namespace TripYari.Core.Metrics
{
    public class ConsoleMetricsProvider : IMetricsProvider
    {
        private readonly object _syncObject = new object();
        private IList<Metric> _metrics;
        private readonly Timer _flushTimer;
        private ILogger Logger { get; }

        public ConsoleMetricsProvider(ILogger logger)
            : this(logger, TimeSpan.FromSeconds(30))
        { }

        public ConsoleMetricsProvider(ILogger logger, TimeSpan flushPeriod)
        {
            Logger = logger;
            _metrics = new List<Metric>(100);
            _flushTimer = new Timer(FlushMetrics, null, flushPeriod, flushPeriod);
        }

        public void Tally(object obj, [CallerMemberName] string methodName = "_", string task = "_", int count = 1)
        {
            string objectName;
            if (obj == null || obj is string)
            {
                objectName = (obj ?? "_").ToString();
            }
            else
            {
                objectName = obj.GetType().Name;
            }

            var newMetric = new TallyMetric(objectName,
                methodName ?? "_",
                task ?? "_",
                count
            );

            lock (_syncObject)
            {
                _metrics.Add(newMetric);
            }
        }

        public void TrackTiming(object obj, TimeSpan elapsedTime, [CallerMemberName] string methodName = "_", string task = "_")
        {
            string objectName;
            if (obj == null || obj is String)
            {
                objectName = (obj ?? "_").ToString();
            }
            else
            {
                objectName = obj.GetType().Name;
            }

            var newMetric = new TimingMetric(objectName,
                methodName ?? "_",
                task ?? "_",
                elapsedTime
                );

            lock (_syncObject)
            {
                _metrics.Add(newMetric);
            }
        }

        public TimingMetric BeginTiming(object obj, [CallerMemberName] string methodName = "_", string task = "_")
        {
            string objectName;
            if (obj == null || obj is String)
            {
                objectName = (obj ?? "_").ToString();
            }
            else
            {
                objectName = obj.GetType().Name;
            }

            var newMetric = new TimingMetric(objectName,
                methodName ?? "_",
                task ?? "_");

            lock (_syncObject)
            {
                _metrics.Add(newMetric);
            }

            return newMetric;
        }

        public void FlushMetrics()
        {
            FlushMetrics(null);
        }

        private void FlushMetrics(object state)
        {
            try
            {
                IList<Metric> flushMetrics;
                lock (_syncObject)
                {
                    if (_metrics.Count == 0)
                    {
                        return;
                    }

                    _metrics.Where(fm => fm.IsCompleted)
                        .ToList()
                        .ForEach(fm => fm.IsFlushed = true);

                    flushMetrics = _metrics.Where(fm => fm.IsFlushed).ToList();
                    _metrics = _metrics.Where(fm => !fm.IsFlushed).ToList();
                }

                if (flushMetrics.Count == 0)
                {
                    return;
                }
                
                Logger.LogInfo("==== Begin Tally Metrics  ====");
                foreach (var flushMetric in flushMetrics.Where(m => m is TallyMetric).Cast<TallyMetric>())
                {
                    Logger.LogInfo($"{flushMetric.ObjectName}.{flushMetric.MethodName}.{flushMetric.Task}: {flushMetric.Count}");
                }
                Logger.LogInfo("==== End Tally Metrics    ====");
                
                Logger.LogInfo("==== Begin Timing Metrics ====");
                foreach (var flushMetric in flushMetrics.Where(m => m is TimingMetric).Cast<TimingMetric>())
                {
                    Logger.LogInfo($"{flushMetric.ObjectName}.{flushMetric.MethodName}.{flushMetric.Task}: {flushMetric.ElapsedTime}");
                }
                Logger.LogInfo("==== End Timing Metrics   ====");
            }
            catch (Exception ex)
            {
                Logger.LogError("Error while logging Time Metrics", ex);
            }
        }

        public void Dispose()
        {
            FlushMetrics(null);
            _flushTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
}
