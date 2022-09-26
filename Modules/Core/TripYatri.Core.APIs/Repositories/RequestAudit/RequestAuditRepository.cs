using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripYatri.Core.Data;
using Microsoft.Extensions.DependencyInjection;
using TripYatri.Core.Data.Sevices;
using TripYatri.Core.Base.Providers.Metrics;

namespace TripYatri.Core.API.Repositories.RequestAudit
{
    public class RequestAuditRepository : IRequestAuditRepository
    {
        private readonly IMetricsProvider _metricsProvider;
        private readonly IDapperService _dapperProvider;

        public RequestAuditRepository(
            IServiceProvider serviceProvider,
            IDapperService dapperProvider)
        {
            _dapperProvider = dapperProvider;
            _metricsProvider = serviceProvider.GetRequiredService<IMetricsProvider>();
        }

        public async Task Save(RequestAudit requestAudit)
        {
            await Save(new[] {requestAudit});
        }

        public async Task Save(IEnumerable<RequestAudit> requestAudits)
        {
            using var _ = _metricsProvider.BeginTiming(this);

            var modifiedRows = await _dapperProvider.ExecuteAsync(
                new Database { Name= "AuditWriter" },
                $"INSERT INTO request_audit_log (" +
                "  `application`," +
                "  `build_version`," +
                "  `request_id`," +
                "  `start_time`," +
                "  `elapsed_time`," +
                "  `client_ip`," +
                "  `owner_id`," +
                "  `client_id`," +
                "  `account_did`," +
                "  `user_did`," +
                "  `controller`," +
                "  `action`," +
                "  `method`," +
                "  `path`," +
                "  `query`," + 
                "  `payload`," + 
                "  `status_code`) " +
                $"VALUES (" +
                $"  @{nameof(RequestAudit.Application)}," +
                $"  @{nameof(RequestAudit.BuildVersion)}," +
                $"  @{nameof(RequestAudit.RequestId)}," +
                $"  @{nameof(RequestAudit.StartTime)}," +
                $"  @{nameof(RequestAudit.ElapsedTime)}," +
                $"  @{nameof(RequestAudit.ClientIp)}," +
                $"  @{nameof(RequestAudit.OwnerId)}," +
                $"  @{nameof(RequestAudit.ClientId)}," +
                $"  @{nameof(RequestAudit.AccountDid)}," +
                $"  @{nameof(RequestAudit.UserDid)}," +
                $"  @{nameof(RequestAudit.Controller)}," +
                $"  @{nameof(RequestAudit.Action)}," +
                $"  @{nameof(RequestAudit.Method)}," +
                $"  @{nameof(RequestAudit.Path)}," +
                $"  @{nameof(RequestAudit.Query)}," +
                $"  @{nameof(RequestAudit.Payload)}," +
                $"  @{nameof(RequestAudit.StatusCode)}" +
                $");",
                requestAudits);

            _metricsProvider.Tally(this, task: "BatchSize", count: requestAudits.Count());

            if (modifiedRows == 0)
                throw new Exception("Failed to insert into request_audit_log. No modified rows.");
        }
    }
}