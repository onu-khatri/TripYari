using System;

namespace TripYatri.Core.Data
{
    public interface IRepositoryDataContext : IDisposable
    {
        T GetRepository<T>() where T : IDataRepository;
        void BeginTransactions();
        void Commit();
        void Rollback();
    }
}
