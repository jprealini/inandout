namespace InAndOut
{
    static class Global
    {
        private static string _appUser = "";

        private static int _appUserId;

        private static bool _appUserIsAdmin;

        public static string appUser
        {
            get { return _appUser; }
            set { _appUser = value; }
        }

        public static int appUserId
        {
            get { return _appUserId; }
            set { _appUserId = value; }
        }

        public static bool appUserIsAdmin
        {
            get { return _appUserIsAdmin; }
            set { _appUserIsAdmin = value; }
        }
    }
}
