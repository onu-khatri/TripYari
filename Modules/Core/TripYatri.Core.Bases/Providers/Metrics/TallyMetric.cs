using System;

namespace TripYatri.Core.Base.Providers.Metrics
{
    public class TallyMetric : Metric
    {
        public int Count { get; private set; }
        
        public TallyMetric(string objectName, string methodName, string task, int count = 1) 
            : base(objectName, methodName, task)
        {
            Count = count;
        }

        public TallyMetric(DateTime startTime, string objectName, string methodName, string task, int count = 1) 
            : base(startTime, objectName, methodName, task)
        {
            Count = count;
        }
    }
}