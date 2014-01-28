using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace BasicHttpIdsrv.Security
{
    public class ViewClaims : List<ViewClaim>
    {
        public static ViewClaims GetAll(ClaimsPrincipal principal)
        {
            var claims = new List<ViewClaim>(
                from c in principal.Claims
                select new ViewClaim
                {
                    Type = c.Type,
                    Value = c.Value
                });

            var vc = new ViewClaims();
            vc.AddRange(claims);

            return vc;
        }
    }

    public class ViewClaim
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}