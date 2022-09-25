using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TripYatri.Core.Base.Providers.Logger;

namespace TripYatri.Core.Base.Collections
{
    public class MemoryBuffer<T> : IDisposable
    {
        private readonly RuntimeEnvironment _runtimeEnvironment;
        private Func<IList<T>, Task> _flusher;
        private readonly object _syncObject = new object();
        private IList<BufferEntry> _data;
        private readonly Timer _flushTimer;
        private readonly int _flushBatchSize;
        private readonly bool _enableAsyncFlush;

        public MemoryBuffer(
            RuntimeEnvironment runtimeEnvironment,
            Func<IList<T>, Task> flusher)
            : this(runtimeEnvironment, 
                flusher, 
                TimeSpan.FromSeconds(30), 
                1000,
                false)
        {
        }

        public MemoryBuffer(
            RuntimeEnvironment runtimeEnvironment,
            Func<IList<T>, Task> flusher, 
            TimeSpan flushPeriod, 
            int flushBatchSize,
            bool enableAsyncFlush)
        {
            _runtimeEnvironment = runtimeEnvironment;
            _flusher = flusher;
            _data = new List<BufferEntry>(100);
            _flushTimer = new Timer(FlushData, null, flushPeriod, flushPeriod);
            _flushBatchSize = flushBatchSize;
            _enableAsyncFlush = enableAsyncFlush;
        }

        public void Add(T data)
        {
            lock (_syncObject)
            {
                _data.Add(new BufferEntry(data));
            }
        }

        public void FlushData()
        {
            FlushData(null);
        }

        private void FlushData(object state)
        {
            try
            {
                IList<BufferEntry> flushData;
                lock (_syncObject)
                {
                    if (!_data.Any())
                    {
                        return;
                    }

                    _data.ToList()
                        .ForEach(fm => fm.IsFlushed = true);

                    flushData = _data.Where(fm => fm.IsFlushed).ToList();
                    _data = _data.Where(fm => !fm.IsFlushed).ToList();
                }

                var flushBatches = flushData
                    .Select((bufferEntry, index) => new {BufferEntry = bufferEntry, Index = index})
                    .GroupBy(a => a.Index / _flushBatchSize);

                if (_enableAsyncFlush)
                {
                    var asyncTasks = flushBatches
                        .Select(flushBatch => 
                            _flusher?.Invoke(flushBatch.Select(fd => fd.BufferEntry.Data).ToList()))
                        .ToList();

                    Task.WaitAll(asyncTasks.Where(t => t != null).ToArray(), TimeSpan.FromMinutes(1));
                }
                else
                {
                    foreach (var flushBatch in flushBatches)
                    {
                        _flusher?.Invoke(flushBatch.Select(fd => fd.BufferEntry.Data).ToList())
                            .ConfigureAwait(false)
                            .GetAwaiter()
                            .GetResult();
                    }
                }
            }
            catch (Exception ex)
            {
                var logger = new ConsoleLogger(new RuntimeContext(_runtimeEnvironment));
                logger.LogError($"Error while flushing {typeof(T).Name}", ex);
            }
        }

        public void Dispose()
        {
            FlushData(null);
            _flushTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _flusher = null;
        }

        private class BufferEntry
        {
            public T Data { get; private set; }
            public bool IsFlushed { get; set; }

            public BufferEntry(T data)
            {
                Data = data;
            }
        }
    }
}