using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProjectCDM.Data.Models
{
    public class TestStep
    {
        public TestStep()
        {
            ShowedImages = new List<Image>();
        }
        public List<Image> ShowedImages { get; set; }
        public int ChosenStyleId { get; set; }
    }
}
