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
            if(Session["Test"]==null)//check registration
                return RedirectToAction("Index");

            //var testChoises = new List<TestChoise>();

            //foreach (var style in _imgRepo.GetAllStyles())
            //{
            //    testChoises.Add(new TestChoise() { Count = 0, StyleId = style.Id });
            //}
            //((Test) Session["Test"]).TestChoises = testChoises;
            //Session["Count"] = 0;//count of test choises
            return View();
        }

        public ActionResult TestPartial(int? id)
        {

            if (!ControllerContext.IsChildAction && (!Request.IsAjaxRequest()))
            {
                return RedirectToAction("Test");
            }

            //if (id != null)
            //{
            //    var chosenStyle = ((Test)Session["Test"]).TestChoises.Find(x => x.StyleId == id);
            //    chosenStyle.Count++;
            //    Session["Count"] = (int) Session["Count"] + 1;
            //}

            //Random rnd = new Random();
            //int styleMaxId = _imgRepo.GetAllStyles().Count;

            //int imgCount = 2;//Count of images in one view
            //List<Image> links = new List<Image>(imgCount);
            //for (int i = 1; i <= imgCount; i++)
            //{
            //    int styleId = rnd.Next(1, styleMaxId + 1);
            //    int imageId = rnd.Next(1, _imgRepo.GetImagesByStyleId(styleId).Count + 1);
            //    var image = _imgRepo.GetImageById(styleId, imageId);
            //    links.Add(image);
            //}
            List<Image> links = new List<Image>(2);
            for (int i = 1; i <= 2; i++)
            {
                var image = new Image();
                links.Add(image);
            }

            return PartialView(links);
        }
    }
}