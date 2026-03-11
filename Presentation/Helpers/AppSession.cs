using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Helpers
{
    public static class AppSession
    {
        public static User CurrentUser { get; private set; }

        public static int CurrentUserId
        {
            get { return CurrentUser != null ? CurrentUser.UserId : -1; }
        }

        public static bool IsAuthenticated
        {
            get { return CurrentUser != null; }
        }

        public static void SetCurrentUser(User user)
        {
            CurrentUser = user;
        }

        public static void Clear()
        {
            CurrentUser = null;
        }
    }
}
