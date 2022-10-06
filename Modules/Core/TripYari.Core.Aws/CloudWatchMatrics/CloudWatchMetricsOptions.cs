namespace TripYari.Core.Aws.CloudWatchMetrics
{
    public class CloudWatchMetricsOptions
    {
        public string Application { get; set; } = "Unknown";
        public bool EnableStatistics { get; set; } = false;

        public double SamplingRate { get; set; } = 1.0d;

        /// <summary>
        /// Max 150
        /// </summary>
        public int MaxDataPointsPerMetric { get; set; } = 50;

        /// <summary>
        /// Max 20
        /// </summary>
        public int MaxMetricsPerRequest { get; set; } = 10;
    }
}
