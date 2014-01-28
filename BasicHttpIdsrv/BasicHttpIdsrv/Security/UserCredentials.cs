using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasicHttpIdsrv.Security
{
    public static class UserCredentials
    {
        public static bool Validate(string username, string password)
        {
            return (username == password);
        }
    }
}