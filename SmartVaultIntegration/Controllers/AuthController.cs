using System.Web.Mvc;

namespace ASPNET_MVC_HelloWorldSVRest.Controllers
{
    public class AuthController : Controller
    {
        //
        // GET: /Auth/

        public ActionResult Index(string access_token, string email)
        {
            Models.SmartVault smartVault = (Models.SmartVault)Session["smartVault"];
                
            smartVault.AuthenticateWithToken(email, access_token);

            return Redirect("/UploadFile");
        }
    }
}
