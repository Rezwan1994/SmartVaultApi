using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPNET_MVC_HelloWorldSVRest.Controllers
{
    public class UploadFileController : Controller
    {
        //
        // GET: /UploadFile/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int? id)
        {
            Models.SmartVault smartVault = (Models.SmartVault)Session["smartVault"];

            string remoteFolder = GetRemoteFolder(smartVault);

            HttpPostedFileBase uploadedFile = Request.Files[0];

            smartVault.UploadFile(uploadedFile.InputStream,
                GetRemoteFolder(smartVault), uploadedFile.FileName);

            Session["remoteFile"] = GetRemoteFolder(smartVault) + "/" + uploadedFile.FileName;

            return new RedirectResult("/DownloadFile");
        }

        public string GetRemoteFolder(Models.SmartVault smartVault)
        {
            var rootNode = smartVault.BrowsePath("/nodes/pth");
            string account = rootNode.Message.ChildrenList[0].Name;

            return String.Format("/nodes/pth/{0}/My First Vault/My First folder", account);
        }

    }
}
