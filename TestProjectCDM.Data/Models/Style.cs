using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProjectCDM.Data.Models
{
    public class Style
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public List<Image> Images { get; set; }
    }
}
