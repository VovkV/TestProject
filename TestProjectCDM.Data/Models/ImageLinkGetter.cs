using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProjectCDM.Data.Interfaces;

namespace TestProjectCDM.Data.Models
{
    public class ImageLinkGetter : IImageLinkGetter
    {
        private Random _rnd;
        public ImageLinkGetter()
        {
             _rnd = new Random();
        }
        public string GetLink()
        {
            
            return "~/Content/Images/American/american"+_rnd.Next(1,10)+".jpg";
        }
    }
}
