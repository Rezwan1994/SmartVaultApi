using System;
using System.Web.Mvc;

namespace ASPNET_MVC_HelloWorldSVRest.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string email)
        {
            Models.SmartVault smartVault = new Models.SmartVault();
            string url = smartVault.RequestPin(email);

            if (Request.Url != null)
            {
                url += String.Format("&redirect_uri={0}/Auth", Request.Url.GetLeftPart(UriPartial.Authority));
            }

            Session["smartVault"] = smartVault;

            return new RedirectResult(url);
        }

    }
}
