using System.Web;
using ResourceBibleStudy.Models;

namespace ResourceBibleStudy.Helpers
{
    public class UserSession
    {
        private UserSession() { }

        public static UserSession Current
        {
            get
            {
                var session = SessionWrapper.GetFromSession<UserSession>(SessionKeyManager.UserSessionKey);
                if (session != null) return session;

                session = new UserSession { LoggedInUser = null };
                SessionWrapper.SetInSession(SessionKeyManager.UserSessionKey, session);
                return session;
            }
        }
        public static void Reset()
        {
            HttpContext.Current.Session.Remove(SessionKeyManager.UserSessionKey);
        }


        public UserDetail   LoggedInUser { get; set; }


    }
}