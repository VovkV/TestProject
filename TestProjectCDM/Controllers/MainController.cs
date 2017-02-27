using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using TestProjectCDM.Data.Interfaces;
using TestProjectCDM.Data.Models;

namespace TestProjectCDM.Controllers
{
    public class MainController : Controller
    {
        private IImageRepository _imgRepo;
        private ITestsRepository _testsRepo;
        private int _imgInStepCount;//Count of images in one view
        private int _choisesMaxCount;//Count of test choises(steps to solve)
        private int _stylesCount;//count of styles in db

        private int _GetNotShownImage(int styleId)
        {
            var chosenImages = ((Test) Session["Test"]).TestChoises.
                Find(x => x.StyleId == styleId).ShowedImages;
            Random rnd = new Random();
            int result;
            int maxId = _imgRepo.GetStyleById(styleId).Images.Count;
            bool wasShown;

            //--- generate random image id and check was it shown
            do
            {
                wasShown = false;
                result = rnd.Next(1, maxId + 1);
                foreach (var imageId in chosenImages)
                    if (imageId == result)
                        wasShown = true;
            } while (wasShown);
            //---

            //--- Add generated image id to session
            ((Test)Session["Test"]).TestChoises.
                Find(x => x.StyleId == styleId).ShowedImages.Add(result);
            //---

            return result;
        }

        public MainController(IImageRepository imageRepository, ITestsRepository testsRepository)
        {
            _imgRepo = imageRepository;
            _testsRepo = testsRepository;

            _imgInStepCount = 2;
            _choisesMaxCount = 10;
            _stylesCount = _imgRepo.GetAllStyles().Count;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string username)
        {
            //--- Check was captcha solved
            var response = Request["g-recaptcha-response"];
            string secretKey = "6Lfl0RYUAAAAAP9JdS_aHxhlk74ojdTcBT8gIPR1";
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            //---

            if (!status)//if captcha wasn't solved
                return RedirectToAction("Index");

            //--- Add username and styles id to test information 
            var testChoises = new List<TestChoise>();

            foreach (var style in _imgRepo.GetAllStyles())
            {
                testChoises.Add(new TestChoise() { Count = 0, StyleId = style.Id });
            }
            var sessionTest = new Test() { Username = username, TestChoises = testChoises };
            //---

            //--- Add test information and count of steps to session
            Session["Test"] = sessionTest;
            Session["Count"] = 0;
            //---

            return RedirectToAction("Test");    
        }

        public ActionResult Test()
        {
            if(Session["Test"]==null)//check registration
                return RedirectToAction("Index");

            return View();
        }

        public ActionResult TestPartial(int? id)
        {
            //--- Block the transition from a direct link
            if (!ControllerContext.IsChildAction && !Request.IsAjaxRequest())
            {
                return RedirectToAction("Test");
            }
            //---

            //--- Add user choice to session and increment count of steps
            if (id != null)
            {
                ((Test)Session["Test"]).TestChoises.Find(x => x.StyleId == id).Count++;
                Session["Count"] = (int)Session["Count"] + 1;
            }
            //---

            var result = new List<Image>(_imgInStepCount);

            //
            var choises = ((Test)Session["Test"]).TestChoises;
            if ((int) Session["Count"] >= _choisesMaxCount)
            {
                var winners = choises.FindAll(x => x.Count == choises.Max(y => y.Count));
                if (winners.Count == 1)
                {
                    Session["WinnerId"] = winners.First().StyleId;
                    return RedirectToAction("Result");
                }
                else
                {
                    for (int i = 0; i < _imgInStepCount; i++)
                    {
                        int styleId = winners[i].StyleId;
                        int imageId = _GetNotShownImage(styleId);
                        var image = _imgRepo.GetImageById(styleId, imageId);

                        result.Add(image);
                    }
                }
            }
            else
            {
                Random rnd = new Random();
                for (int i = 0; i < _imgInStepCount; i++)
                {
                    int styleId = rnd.Next(1, _stylesCount + 1);
                    int imageId = _GetNotShownImage(styleId);
                    var image = _imgRepo.GetImageById(styleId, imageId);

                    result.Add(image);
                }
            }

            return PartialView(result);
        }
        public ActionResult Result()
        {
            if (Session["WinnerId"] == null)
                return RedirectToAction("Index");

            var test = ((Test) Session["Test"]);
            test.Id = new Guid();
            test.CompleteTime = DateTime.Now;
            _testsRepo.AddTest(test);

            int styleId = ((int)Session["WinnerId"]);
            var result = _imgRepo.GetStyleById(styleId);

            Session.Clear();

            return PartialView(result);
        }
    }
}