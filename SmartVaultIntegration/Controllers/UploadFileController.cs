using ASPNET_MVC_HelloWorldSVRest.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static ASPNET_MVC_HelloWorldSVRest.Models.NonceResponse;

namespace ASPNET_MVC_HelloWorldSVRest.Controllers
{
    public class UploadFileController : Controller
    {
        //
        // GET: /UploadFile/

        public class MyViewModel
        {
            public string Message { get; set; }
        }

        public ActionResult Index()
        {
            MyViewModel model = new MyViewModel
            {
                Message = TempData["message"] as string,
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(int? id)
        {
            Models.SmartVault smartVault = (Models.SmartVault)Session["smartVault"];
            HttpPostedFileBase uploadedFile = Request.Files[0];

            if (uploadedFile != null)
            {
                smartVault.UploadFile(uploadedFile.InputStream, GetRemoteFolder(smartVault), uploadedFile.FileName);
                Session["remoteFile"] = GetRemoteFolder(smartVault) + "/" + uploadedFile.FileName;
                TempData["message"] = "File uploaded successfully.";
                return RedirectToAction("index");
            }

            TempData["message"] = null;
            return RedirectToAction("index");
        }

        public string GetRemoteFolder(Models.SmartVault smartVault)
        {
            var rootNode = smartVault.BrowsePath("/nodes/pth");
            string account = rootNode.Message.ChildrenList[0].Name;

            return String.Format("/nodes/pth/{0}/My First Vault/My First folder", account);
        }
        string token = "";
        public async System.Threading.Tasks.Task<ActionResult> AuthorizationAsync()
        {
    
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            var baseAddress = new Uri("https://rest.smartvault.com/");
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                using (var content = new StringContent("{  \"client_id\": \"Reza10\"}", System.Text.Encoding.Default, "application/json"))
                {
                    using (var response = await httpClient.PostAsync("auto/auth/nonce/1", content))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                 
                        var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<NonceRoot>(responseData);
                        TokenGenerator generator = new TokenGenerator();
                        string PrivateKey = @"-----BEGIN RSA PRIVATE KEY-----
                                        MIIEogIBAAKCAQEAudQiZoUPz+QioCAz+k6254m5R6TIcUNVfnGJZdyvJQgzTaMb
                                        C3P7ihRrL+ASoEdlhDOk6bIk5e/SDCm/3Q77l4Hb4jdahE3Mzl5LoZyrFCbu/mIw
                                        34D5IukorZ0eyZcPV+X08bpczJQA8cusk7GdwAqz9dYBgOcZTldx+0qi57o5rM/E
                                        8FV2X+H5lJY3G4s4Hm4rXMAfizN3X0uHz8jVrfIZlqL1cMRzRYnYVlMmINRmZm0B
                                        ZXS6UPN/eMnVC5utrOYCuDuTBV4YtL6Kb1CIc0X7gQgAz0GKj/dgxnOa2rdvE5Re
                                        l24FhAduTDzWWrmTErIWqYaG2qD5JtvvLAKUKwIDAQABAoIBAE8Vk8pTqKYN7hp9
                                        egF1zGt/dmWiIFfrkw/r29CI+dTlx4er+Y+HPa7G/9LxJpwlpnN70xxN31woExlw
                                        DyJSBQq2jlCYxCH4RkgxYziVR02dQCrsuOX1qpGL7u7sJjUe373FtNfHO0UpW7mL
                                        t5dniL45D9t4x00HjQKFPpH2cpvxMAqu0MHLiZh223+aSknx8zHXWJX+p3kfXSgW
                                        asgkbAtE4kJPdkCmejcGmXJZ2HT6tB0pFMchZwQRTQl1G5K3JdyzayEoZYMeH6YO
                                        u5H2SLy0uLwJaL5N5CtIdvdHqR/1gXvshXQaaMkJIov+z2GKLQ1q1MilYc+eVdcV
                                        sp+Go1ECgYEA6ilShAMa30GInjjt7ah09/Q+DUI6LOB5c3xAKmNqWERecmtUMZyj
                                        uqvdf4Wc4rtPeyp0oYqUgPIHYibm8WS6jVG8tR9OAD+Ctg0YnGP4zGOfUOX+Ohr9
                                        aLNOkS30zdQ3f3SrxJ8GQ+sgiwOpV8OVwJ4XbRLM5WmYBkCO69WgUwMCgYEAyyja
                                        Hlhnj9JRkuJfmkiBWFov4AHgY7GvfBqY3P3K85lHT1Gx8ZCC+qdqT9R0QwWXohdh
                                        8BznrP+SfmwgZQtHAN/7a8C/xBpizJoGDSv0oIHRvMy/g6XlVNtJNrEiUqjyH22V
                                        M4ptwDm/iwVgXcYS+VSeKrVxv7jBTkTDDWd/3bkCgYAfVbxHEitMaPahRbkBLqPI
                                        cpTDLXOfgcJ+48eqZHdcrHl/EkHICdAXMNyJLhYaeTpfnwTQgTzyLHE0f0Q0VWhp
                                        dOCt1CyZ9+XD+uiqNO6cW1B8gOqOWAJFOolvABlcWeO7WJ+LxkQOXq6SK8FDj1bA
                                        2ecEn5lvsbu4SndVs9aqhQKBgBWadX9OlUyk58m/yrzEQwTbKkYezPas+WUwCttN
                                        HZzDyuJzZIG0hUkULmjoxagu2w4AgIig+j3aO4C4DMXR6i38o2a06292AtWQ913F
                                        M9ExnNT/zMm6BWX9n45yTZ0OnBiddYUyjjMnsOeenb27B19+EQennb3ubpfdFgTB
                                        4cX5AoGAd6XVeYvjcWoj4+Uwh1jaXZueVohjZ1a722ZAuQMNDFft7r2qD7qzmsXg
                                        GLBJukdnP+Mv/OwMVS91EwZiQRzQsFipHpT35vqj5jRhvkT6Utv2CfAdZHnyFQ/p
                                        shhTJK/GBA7/280DLiNkQxzT8VhoxJBuferchk58rvGw5S8ccVc=
                                        -----END RSA PRIVATE KEY-----";

                        token = generator.Generate(PrivateKey,"Reza10",obj.message.code);
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                        var baseAddress1 = new Uri("https://rest.smartvault.com/");
                        //string tokenjson = string.Format(@"{
                        //  'token': 'U0xGMDAGUmV6YTEwFlU3bXhXU1RPQ1UtbWhGNGtKeUhpeEFcygAVoFyTac6PxLZYjlWw09iip4IEnVmvjTRJsqzLlqtz36nWT1HnmRpTG5NArey6tunfIo+Jc17I/fAQ3gtVUQ8ouJ31xDHqpABFczmZDIWdQ72aF+VV+d5BRY9VvQVXyArYC0Bu583Gk7O73TH3U5OFfPZlhJrWEGboVNfVkR5xLucJE1YQMzGjFyi3h5ZpTI8gF3ExHBsDwGsAExehYXuoCGuOjW8Ur4g3EnJGznS2Gkpe+34wxBKvnOHecRUljvPQ8xtEhUZfVfYiwtlXyCK7sMja1Xblo1Ih4iNRYDI30DoQlr7FA62O7+B2M03Pn3v8yyVyqu9cp2FcdI4f'
                        //}", token);
                        using (var httpClient1 = new HttpClient { BaseAddress = baseAddress1 })
                        {

                            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                            using (var content1 = new StringContent("{  \"token\": \""+ token + "\"}", System.Text.Encoding.Default, "application/json"))
                            {
                                using (var response1 = await httpClient1.PostAsync("auto/auth/ctoken/1", content1))
                                {
                                    string responseData1 = await response1.Content.ReadAsStringAsync();
                                    var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<NonceRoot>(responseData);
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        public async System.Threading.Tasks.Task<ActionResult> GenerateDTocken(string token)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var baseAddress = new Uri("https://rest.smartvault.com/");
            string tokenjson = string.Format(@"{
  'token': '{0}'
}", token);
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                using (var content = new StringContent(tokenjson, System.Text.Encoding.Default, "application/json"))
                {
                    using (var response = await httpClient.PostAsync("auto/auth/ctoken/1", content))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return null;
        }


    }
}
