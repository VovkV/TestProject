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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        public PartialViewResult TestPartial()
        {
            int imgCount = 2;//Count of images in test
            List<string> links = new List<string>(imgCount);
            for (int i = 0; i < imgCount; i++)
            {
                links.Add(_imageLinkGetter.GetLink());
            }

            return PartialView(links);
        }
    }
}