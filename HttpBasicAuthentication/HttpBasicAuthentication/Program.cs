using System;
using System.IdentityModel.Tokens;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Thinktecture.IdentityModel.Clients;
using Thinktecture.IdentityModel.Extensions;

namespace HttpBasicAuthentication
{
    class Program
    {
        static Uri _baseAddress = new Uri("https://localhost:44301/");

        static void Main(string[] args)
        {
            // IMPORTANT NOT TO DO THIS ON PROD
            // WE ARE ONLY DOING THIS DUE TO SELF SIGNED CERT
            System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();

            var jwt = BuildJwtToken();

            DecodeToken(jwt);

            CallService(jwt);

            Console.ReadKey();
        }


        public static string BuildJwtToken()
        {
            var client = new OAuth2Client(
                new Uri("https://localhost/idsrv/issue/oauth2/token"),
                "BasicHttpIdsrvTest",
                "fRcnE4PvE5pMu4Xj0gxzKs0/iSYtGxn+nhM+Cu+zr10=");


            var response_admin = client.RequestAccessTokenUserName("admin", "Verrus123", "https://localhost:44301/");
            var response_nino = client.RequestAccessTokenUserName("nino", "Verrus123", "https://localhost:44301/");
            return response_admin.AccessToken;
        }


        public static void CallService(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("token");
            }

            var client = new HttpClient
                {
                    BaseAddress =  _baseAddress
                };

            client.SetBearerToken(token);

            var response = client.GetAsync("api/values").Result;

            response.EnsureSuccessStatusCode();

            var j = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);

            Console.WriteLine(response);
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine(j);

        }

        private static void DecodeToken(string encToken)
        {
            "Raw token:".ConsoleYellow();
            encToken.ConsoleRed();
            Console.WriteLine("\n");

            var token = new JwtSecurityToken(encToken);

            "Token claims:".ConsoleYellow();
            foreach (var claim in token.Claims)
            {
                Console.WriteLine(" " + claim.Type);
                string.Format("  {0}\n", claim.Value).ConsoleGreen();
            }
            Console.WriteLine("\n");
        }

        /// <summary>
        /// DISABLE THE CERTIFICATE VALIDATION
        /// </summary>

        public class MyPolicy : ICertificatePolicy
        {
            public bool CheckValidationResult(ServicePoint srvPoint,
              X509Certificate certificate, WebRequest request,
              int certificateProblem)
            {
                //Return True to force the certificate to be accepted.
                return true;
            }
        }
    }
}
