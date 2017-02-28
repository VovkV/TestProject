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
        private int _stepsMaxCount;//Count of test choises(steps to solve)
        private int _stylesCount;//count of styles in db

        private int _GetNotShownImage(int styleId)
        {
            var steps = ((Test)Session["Test"]).Steps;
            List<int> showedImages = new List<int>();
            
            foreach (var step in steps)
            {
                foreach (var image in step.ShowedImages.Where(x => x.StyleId == styleId))
                {
                    showedImages.Add(image.Id);
                }
            }
            
            Random rnd = new Random();
            int result;
            int maxId = _imgRepo.GetStyleById(styleId).Images.Count;
            bool wasShown;

            //--- generate random image id and check was it shown
            do
            {
                wasShown = false;
                result = rnd.Next(1, maxId + 1);
                foreach (var imageId in showedImages)
                    if (imageId == result)
                        wasShown = true;
            } while (wasShown);
            //---

            return result;
        }

        private int _GetNotInListStyle(List<Image> list)
        {
            Random rnd = new Random();
            int result;
            bool inList;

            //--- generate random style id and check is it exist in list
            do
            {
                inList = false;
                result = rnd.Next(1, _stylesCount + 1);
                foreach (var image in list)
                    if (image.StyleId == result)
                        inList = true;
            } while (inList);
            //---

            return result;
        }

        public MainController(IImageRepository imageRepository, ITestsRepository testsRepository)
        {
            _imgRepo = imageRepository;
            _testsRepo = testsRepository;

            _imgInStepCount = 2;
            _stepsMaxCount = 10;
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

            //if (!status)//if captcha wasn't solved
            //    return RedirectToAction("Index");

            //--- Add username to test information 
            var sessionTest = new Test() { Username = username};
            //---

            //--- Add test information and count of steps to session
            Session["Test"] = sessionTest;
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
            if (!ControllerContext.IsChildAction)
                if (!Request.IsAjaxRequest() || Session["Test"] == null)
                    return RedirectToAction("Test");
            //---

            //--- Add user choice to session
            if (id != null)
            {
                ((Test)Session["Test"]).Steps.Last().ChosenStyleId = (int)id;
            }
            //---

            var result = new List<Image>(_imgInStepCount);

            //--- If step is last
            if (((Test)Session["Test"]).Steps.Count >= _stepsMaxCount)
            {
                var winners = ((Test)Session["Test"]).GetWinnersId();
                if (winners.Count == 1)
                {
                    Session["WinnerId"] = winners.First();
                    return RedirectToAction("Result");
                }

                //--- If styles with biggest choises more then one -> will work till style will be one
                else
                {
                    var step = new TestStep();

                    for (int i = 0; i < _imgInStepCount; i++)
                    {
                        int styleId = winners[i];
                        int imageId = _GetNotShownImage(styleId);

                        var image = _imgRepo.GetImageById(styleId, imageId);

                        step.ShowedImages.Add(image);
                        result.Add(image);
                    }
                    ((Test) Session["Test"]).Steps.Add(step); //Add new step
                }
                //---
            }
            //---

            //--- Generate two random img from different styles
            else
            {
                var step = new TestStep();
                Random rnd = new Random();
                for (int i = 0; i < _imgInStepCount; i++)
                {
                    int styleId = _GetNotInListStyle(result);
                    int imageId = _GetNotShownImage(styleId);

                    var image = _imgRepo.GetImageById(styleId, imageId);

                    step.ShowedImages.Add(image);
                    result.Add(image);
                }
                ((Test)Session["Test"]).Steps.Add(step);//Add new step
            }
            //---

            return PartialView(result);
        }

        public ActionResult PrevStep()
        {
            //--- Block the transition from a direct link
            if (!Request.IsAjaxRequest() || Session["Test"] == null)
                return RedirectToAction("Test");
            //---

            var steps = ((Test) Session["Test"]).Steps;
            var result = new List<Image>(_imgInStepCount);
            steps.RemoveAt(steps.Count - 1);
            var lastStep = steps.Last();

            var step = new TestStep();

            for (int i = 0; i < _imgInStepCount; i++)
            {
                int styleId = lastStep.ShowedImages[i].StyleId;
                int imageId = lastStep.ShowedImages[i].Id;

                var image = _imgRepo.GetImageById(styleId, imageId);

                step.ShowedImages.Add(image);
                result.Add(image);
            }

            steps.RemoveAt(steps.Count - 1);
            ((Test)Session["Test"]).Steps.Add(step); //Add new step

            return PartialView("TestPartial",result);
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

            Session.Abandon();

            return PartialView(result);
        }
    }
}