using System.Collections.Generic;
using System.Threading.Tasks;
using TripYatri.Core.Data;

namespace TripYatri.Core.API.Repositories
{
    public interface IRequestAuditRepository : IDataRepository
    {
        Task Save(RequestAudit.RequestAudit requestAudit);
        Task Save(IEnumerable<RequestAudit.RequestAudit> requestAudits);
    }
}