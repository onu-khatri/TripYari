using System;
using System.Data;

namespace TripYatri.Core.Data
{
    public interface ISQLDataContext : IDisposable
    {
        string GetConnectionString(Database db);
        IDbConnection GetConnection(Database db);
        IDbTransaction GetTransaction(Database db);
        void BeginTransactions();
        void Commit();
        void Rollback();
    }
}
