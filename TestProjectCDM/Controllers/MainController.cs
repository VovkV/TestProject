using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestProjectCDM.Data.Interfaces;

namespace TestProjectCDM.Controllers
{
    public class MainController : Controller
    {
        private IImageLinkGetter _imageLinkGetter;
        public MainController(IImageLinkGetter imageLinkGetter)
        {
            _imageLinkGetter = imageLinkGetter;
        }
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            int imgCount = 2;//Count of images in test
            List<string> links = new List<string>(imgCount);
            for (int i = 0; i < imgCount; i++)
            {
                links.Add(_imageLinkGetter.GetLink());
            }

            return View(links);
        }
    }
}