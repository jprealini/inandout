using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InAndOut
{
    static class Global
    {
        private static string _appUser = "";

        private static int _appUserId;

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
    }
}
