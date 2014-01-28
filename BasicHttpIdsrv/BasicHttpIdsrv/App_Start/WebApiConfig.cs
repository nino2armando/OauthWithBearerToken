using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BasicHttpIdsrv.Security;
using Thinktecture.IdentityModel.Tokens.Http;

namespace BasicHttpIdsrv
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var authentication = CreateAuthenticationConfiguration();

            config.MessageHandlers.Add(new AuthenticationHandler(authentication));

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
        }

        private static AuthenticationConfiguration CreateAuthenticationConfiguration()
        {
            var authentication = new AuthenticationConfiguration
            {
                ClaimsAuthenticationManager = new ClaimsTransformer(),
                RequireSsl = false,
                EnableSessionToken = true
            };

            authentication.AddJsonWebToken(
                issuer: "http://identityserver.v2.thinktecture.com/trust/idsrv",
                audience: "https://localhost:44301/",
                signingKey: "8hlN4y8TZBYNLtUrhvLPUfLRjx3KWMo24JdAurlcRMs=");


            

            return authentication;
        }
    }
}
