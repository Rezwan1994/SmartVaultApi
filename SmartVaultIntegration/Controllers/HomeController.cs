using ASPNET_MVC_HelloWorldSVRest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            Session["smartVault"] = smartVault;

            string url = smartVault.RequestPin(email);

            url += String.Format("&redirect_uri={0}/Auth",
                Request.Url.GetLeftPart(UriPartial.Authority));

            return new RedirectResult(url);
        }

    }
}
