using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BasicHttpIdsrv.Security;
using Thinktecture.IdentityModel.Extensions;

namespace BasicHttpIdsrv.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ViewClaims Claims()
        {

            var request = Request.ToHttpRequestMessage();

            var principal = request.GetClaimsPrincipal();
                
            
            var claims = ViewClaims.GetAll(principal);

            return claims;
        }
         // min < 35 
        [HttpPost]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // clear session cookie 
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            return RedirectToAction("Index", "Home");
        }
    }
}
