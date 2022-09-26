using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TripYatri.Core.API.Repositories;
using TripYatri.Core.Base.Collections;
using TripYatri.Core.Data;
using TripYatri.Core.Base.Providers;
using Microsoft.Extensions.DependencyInjection;
using TripYatri.Core.Base;
using TripYatri.Core.Base.Providers.Metrics;

namespace TripYatri.Core.API.Providers.RequestAudit
{
    public class RequestAuditBufferedProvider : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly MemoryBuffer<Repositories.RequestAudit.RequestAudit> _requestAuditsBuffer;

        public RequestAuditBufferedProvider(
            IServiceProvider serviceProvider,
            RuntimeEnvironment runtimeEnvironment)
        {
            _serviceProvider = serviceProvider;
            _requestAuditsBuffer = new MemoryBuffer<Repositories.RequestAudit.RequestAudit>(
                runtimeEnvironment,
                FlushAudits,
                TimeSpan.FromSeconds(10),
                100,
                false
            );
        }

        public void Audit(Repositories.RequestAudit.RequestAudit requestAudit)
        {
            _requestAuditsBuffer.Add(requestAudit);
        }

        private async Task FlushAudits(IList<Repositories.RequestAudit.RequestAudit> requestAudits)
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var metricsProvider = serviceScope.ServiceProvider.GetRequiredService<IMetricsProvider>();
            var dataContextFactory = serviceScope.ServiceProvider.GetRequiredService<IDataContextFactory>();
            using var dataContext = dataContextFactory.CreateRepositoryDataContext();
            var requestAuditRepository = dataContext.GetRepository<IRequestAuditRepository>();

            using var _ = metricsProvider.BeginTiming(this);
            await requestAuditRepository.Save(requestAudits);
        }

        public void Dispose()
        {
            _requestAuditsBuffer?.Dispose();
        }
    }
}