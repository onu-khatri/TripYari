namespace TripYari.Core.Metrics
{
    public abstract class Metric : IDisposable
    {
        public DateTime StartTime { get; }
        public string ObjectName { get; }
        public string MethodName { get; }
        public string Task { get; }
        public bool IsFlushed { get; set; }

        protected Metric(string objectName, string methodName, string task)
            : this(DateTime.UtcNow, objectName, methodName, task)
        {
        }

        protected Metric(DateTime startTime, string objectName, string methodName, string task)
        {
            StartTime = startTime;
            ObjectName = objectName;
            MethodName = methodName;
            Task = task;
        }

        public virtual bool IsCompleted => true;

        public virtual void Dispose()
        {
        }
    }
}
