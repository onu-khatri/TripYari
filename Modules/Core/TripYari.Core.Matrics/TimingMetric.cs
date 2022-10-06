namespace TripYari.Core.Metrics
{
    public class TimingMetric : Metric
    {
        public TimeSpan? ElapsedTime { get; private set; }

        public TimingMetric(string objectName, string methodName, string task)
            : base(objectName, methodName, task)
        {
            ElapsedTime = null;
        }

        public TimingMetric(string objectName, string methodName, string task, TimeSpan elapsedTime)
            : base(DateTime.UtcNow - elapsedTime, objectName, methodName, task)
        {
            ElapsedTime = elapsedTime;
        }

        public override bool IsCompleted => ElapsedTime != null;
        public double ElapsedMilliseconds => ElapsedTime?.TotalMilliseconds ?? (DateTime.UtcNow - StartTime).TotalMilliseconds;

        public override void Dispose()
        {
            if (ElapsedTime == null)
                ElapsedTime = DateTime.UtcNow - StartTime;
            base.Dispose();
        }
    }
}
