namespace TripYatri.Core.Data
{
    public interface IDataContextFactory
    {
        IRepositoryDataContext CreateRepositoryDataContext();
        ISQLDataContext CreateSQLDataContext();
    }
}
