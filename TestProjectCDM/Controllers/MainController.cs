using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.ApplicationInsights.Web;
using TestProjectCDM.Data.Interfaces;
using TestProjectCDM.Data.Models;

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

        public ActionResult Test()
        {
            //var userId = Guid.NewGuid();
            //Session["UserId"] = userId;
            //Session["ImageLinkGetter"] = new ImageLinkGetter();
            return View();
        }
        public PartialViewResult TestPartial()
        {
            //var t = (IImageLinkGetter)Session["ImageLinkGetter"];
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