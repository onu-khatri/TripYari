namespace TripYatri.Core.Base.Providers.Session
{
    public interface ISessionProvider
    {
        void Set(string key, object data);
        T Get<T>(string key);
    }
}
