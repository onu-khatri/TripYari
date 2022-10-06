using Amazon.CloudWatch;
using Amazon.CloudWatch.Model;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using TripYari.Core.Loggers;
using TripYari.Core.Metrics;
using TripYari.Core.RuntimeContext;
using Metric = TripYari.Core.Metrics.Metric;

namespace TripYari.Core.Aws.CloudWatchMetrics
{
    public class CloudWatchMetricsProvider : IMetricsProvider
    {
        private readonly IRuntimeContextProvider _runtimeEnvironmentProvider;
        private readonly CloudWatchMetricsOptions _options;
        private readonly Random _random = new Random();
        private readonly object _syncObject = new object();
        private IList<Metric> _metrics;
        private readonly Timer _flushTimer;
        private readonly IAmazonCloudWatch _amazonCloudWatch;

        public CloudWatchMetricsProvider(IRuntimeContextProvider runtimeEnvironmentProvider,
            IOptions<CloudWatchMetricsOptions> options, IAmazonCloudWatch amazonCloudWatch)
            : this(runtimeEnvironmentProvider, options, amazonCloudWatch, TimeSpan.FromSeconds(30))
        {
        }

        public CloudWatchMetricsProvider(IRuntimeContextProvider runtimeEnvironmentProvider,
            IOptions<CloudWatchMetricsOptions> options, IAmazonCloudWatch amazonCloudWatch, TimeSpan flushPeriod)
        {
            _runtimeEnvironmentProvider = runtimeEnvironmentProvider;
            _options = options?.Value ?? new CloudWatchMetricsOptions();
            _amazonCloudWatch = amazonCloudWatch;
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

        public void TrackTiming(object obj, TimeSpan elapsedTime, [CallerMemberName] string methodName = "_",
            string task = "_")
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
            if (obj == null || obj is string)
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
                static string RemoveMetricSuffix(string name)
                {
                    return name.EndsWith("Metric") ? name.Substring(0, name.Length - 6) : name;
                }

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

                // Enable sampling on metrics to be flushed
                var samplingRate = _options.SamplingRate;
                flushMetrics = flushMetrics
                    .Where(_ => _random.NextDouble() < samplingRate)
                    .ToList();

                if (flushMetrics.Count == 0)
                {
                    return;
                }

                var maxDataPointsPerMetric = _options.MaxDataPointsPerMetric;
                var maxMetricsPerRequest = _options.MaxMetricsPerRequest;
                var awsMetricDatums = flushMetrics
                    .GroupBy(fm => $"{fm.GetType().Name}.{fm.ObjectName}.{fm.MethodName}.{fm.Task}")
                    .SelectMany(flushMetricNameGroup =>
                    {
                        var firstFlushMetric = flushMetricNameGroup.First();

                        if (_options.EnableStatistics)
                        {
                            switch (firstFlushMetric)
                            {
                                case TimingMetric _:
                                {
                                    var timingFlushMetricNameGroup = flushMetricNameGroup.Cast<TimingMetric>().ToList();
                                    return new[]
                                    {
                                        new MetricDatum
                                        {
                                            Dimensions = new List<Dimension>
                                            {
                                                new Dimension
                                                    {Name = "Environment", Value = _runtimeEnvironmentProvider.ToString()},
                                                new Dimension
                                                {
                                                    Name = "Type",
                                                    Value = RemoveMetricSuffix(firstFlushMetric.GetType().Name)
                                                }
                                            },
                                            MetricName =
                                                $"{firstFlushMetric.ObjectName}.{firstFlushMetric.MethodName}.{firstFlushMetric.Task}",

                                            StatisticValues = new StatisticSet()
                                            {
                                                Maximum = timingFlushMetricNameGroup.Max(fm => fm.ElapsedMilliseconds),
                                                Minimum = timingFlushMetricNameGroup.Min(fm => fm.ElapsedMilliseconds),
                                                SampleCount = flushMetricNameGroup.Count(),
                                                Sum = timingFlushMetricNameGroup.Sum(fm => fm.ElapsedMilliseconds)
                                            },
                                            TimestampUtc = flushMetricNameGroup.Min(fm => fm.StartTime),
                                            Unit = StandardUnit.Milliseconds
                                        }
                                    };
                                }
                                case TallyMetric _:
                                {
                                    var tallyFlushMetricNameGroup = flushMetricNameGroup.Cast<TallyMetric>().ToList();
                                    return new[]
                                    {
                                        new MetricDatum
                                        {
                                            Dimensions = new List<Dimension>
                                            {
                                                new Dimension
                                                    {Name = "Environment", Value = _runtimeEnvironmentProvider.ToString()},
                                                new Dimension
                                                {
                                                    Name = "Type",
                                                    Value = RemoveMetricSuffix(firstFlushMetric.GetType().Name)
                                                }
                                            },
                                            MetricName =
                                                $"{firstFlushMetric.ObjectName}.{firstFlushMetric.MethodName}.{firstFlushMetric.Task}",

                                            StatisticValues = new StatisticSet()
                                            {
                                                Maximum = tallyFlushMetricNameGroup.Max(fm => fm.Count),
                                                Minimum = tallyFlushMetricNameGroup.Min(fm => fm.Count),
                                                SampleCount = flushMetricNameGroup.Count(),
                                                Sum = tallyFlushMetricNameGroup.Sum(fm => fm.Count)
                                            },
                                            TimestampUtc = flushMetricNameGroup.Min(fm => fm.StartTime),
                                            Unit = StandardUnit.Count
                                        }
                                    };
                                }
                                default:
                                    throw new NotSupportedException(
                                        $"Flushing of metric type {firstFlushMetric.GetType().Name} not supported.");
                            }
                        }

                        switch (firstFlushMetric)
                        {
                            case TimingMetric _:
                            {
                                return flushMetricNameGroup
                                    .Cast<TimingMetric>()
                                    .GroupBy(m => m.ElapsedMilliseconds)
                                    .Select((elapsedGroup, index) => new
                                    {
                                        Index = index,
                                        StartTime = elapsedGroup.Min(m => m.StartTime),
                                        ElapsedMilliseconds = elapsedGroup.Key,
                                        Count = elapsedGroup.Count()
                                    })
                                    .GroupBy(a => a.Index / maxDataPointsPerMetric)
                                    .Select(indexGroup => new MetricDatum
                                    {
                                        Dimensions = new List<Dimension>
                                        {
                                            new Dimension
                                                {Name = "Environment", Value = _runtimeEnvironmentProvider.ToString()},
                                            new Dimension
                                            {
                                                Name = "Type",
                                                Value = RemoveMetricSuffix(firstFlushMetric.GetType().Name)
                                            }
                                        },
                                        MetricName =
                                            $"{firstFlushMetric.ObjectName}.{firstFlushMetric.MethodName}.{firstFlushMetric.Task}",
                                        StatisticValues = new StatisticSet(),

                                        Counts =
                                            indexGroup.Select(elapsedGroup => (double) elapsedGroup.Count).ToList(),
                                        Values = indexGroup.Select(elapsedGroup => elapsedGroup.ElapsedMilliseconds)
                                            .ToList(),
                                        TimestampUtc = indexGroup.Min(elapsedGroup => elapsedGroup.StartTime),
                                        Unit = StandardUnit.Milliseconds
                                    });
                            }
                            case TallyMetric _:
                            {
                                return flushMetricNameGroup
                                    .Cast<TallyMetric>()
                                    .GroupBy(m => m.Count)
                                    .Select((elapsedGroup, index) => new
                                    {
                                        Index = index,
                                        StartTime = elapsedGroup.Min(m => m.StartTime),
                                        Count = elapsedGroup.Key,
                                        SampleCount = elapsedGroup.Count()
                                    })
                                    .GroupBy(a => a.Index / maxDataPointsPerMetric)
                                    .Select(indexGroup => new MetricDatum
                                    {
                                        Dimensions = new List<Dimension>
                                        {
                                            new Dimension
                                                {Name = "Environment", Value = _runtimeEnvironmentProvider.ToString()},
                                            new Dimension
                                            {
                                                Name = "Type",
                                                Value = RemoveMetricSuffix(firstFlushMetric.GetType().Name)
                                            }
                                        },
                                        MetricName =
                                            $"{firstFlushMetric.ObjectName}.{firstFlushMetric.MethodName}.{firstFlushMetric.Task}",
                                        StatisticValues = new StatisticSet(),

                                        Counts =
                                            indexGroup.Select(elapsedGroup => (double) elapsedGroup.SampleCount)
                                                .ToList(),
                                        Values = indexGroup.Select(elapsedGroup => (double) elapsedGroup.Count)
                                            .ToList(),
                                        TimestampUtc = indexGroup.Min(elapsedGroup => elapsedGroup.StartTime),
                                        Unit = StandardUnit.Count
                                    });
                            }
                            default:
                                throw new NotSupportedException(
                                    $"Flushing of metric type {firstFlushMetric.GetType().Name} not supported.");
                        }
                    })
                    .ToList();

                var awsMetricGroups = awsMetricDatums.Select((metric, index) => new {Metric = metric, Index = index})
                    .GroupBy(a => a.Index / maxMetricsPerRequest);

                var asyncTasks = new List<Task>();

                using (BeginTiming(this))
                {
                    foreach (var awsMetricGroup in awsMetricGroups)
                    {
                        var awsPutMetricDataRequest = new PutMetricDataRequest
                        {
                            MetricData = awsMetricGroup.Select(a => a.Metric).ToList(),
                            Namespace = $"cds.{_options.Application}"
                        };

                        asyncTasks.Add(_amazonCloudWatch.PutMetricDataAsync(awsPutMetricDataRequest));
                    }

                    Task.WaitAll(asyncTasks.ToArray(), TimeSpan.FromMinutes(1));
                }
            }
            catch (Exception ex)
            {
                var logger = new ConsoleLogger(_runtimeEnvironmentProvider);
                logger.LogError("Error while publishing AWS Custom Time Metrics", ex);
            }
        }

        public void Dispose()
        {
            FlushMetrics(null);
            _flushTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
}