using System;

namespace TripYatri.Core.Data
{
    public class DataContextFactory : IDataContextFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DataContextFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IRepositoryDataContext CreateRepositoryDataContext()
        {
            return new DataContext(_serviceProvider);
        }

        public ISQLDataContext CreateSQLDataContext()
        {
            return new DataContext(_serviceProvider);
        }
    }
}
