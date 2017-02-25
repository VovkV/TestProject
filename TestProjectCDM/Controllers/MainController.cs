using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.ApplicationInsights.Web;
using Newtonsoft.Json.Linq;
using TestProjectCDM.Data.Interfaces;
using TestProjectCDM.Data.Models;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;

namespace TestProjectCDM.Controllers
{
    public class MainController : Controller
    {
        private IImageRepository _imgRepo;
        private ITestsRepository _testsRepo;
        public MainController(IImageRepository imageRepository, ITestsRepository testsRepository)
        {
            _imgRepo = imageRepository;
            _testsRepo = testsRepository;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username)
        {
            var response = Request["g-recaptcha-response"];
            string secretKey = "6Lfl0RYUAAAAAP9JdS_aHxhlk74ojdTcBT8gIPR1";
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");

            if (!status)//if captcha wasn't solved
                return View("Index");

            Session["Test"] = new Test() {Username = username};
            return RedirectToAction("Test");
            

        }
        public ActionResult Test()
        {
            if(Session["Test"]==null)
                return RedirectToAction("Index");

            return View();
        }
        public PartialViewResult TestPartial()
        {
            int imgCount = 2;//Count of images in test
            List<string> links = new List<string>(imgCount);
            for (int i = 0; i < imgCount; i++)
            {
                links.Add("");
            }

            return PartialView(links);
        }
    }
}