namespace UnitTestArticle.Interfaces
{
    public interface ISessionManager
    {
        void Store<T>(string key, T value);
        T Get<T>(string key);
    }
}
