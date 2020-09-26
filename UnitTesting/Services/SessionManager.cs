using System.Web;
using UnitTestArticle.Interfaces;

namespace UnitTestArticle.Services
{
    public class SessionManager : ISessionManager
    {
        public T Get<T>(string key)
        {
            return (T)HttpContext.Current.Session[key];
        }

        public void Store<T>(string key, T value)
        {
            HttpContext.Current.Session[key] = value;
        }
    }
}