using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace TestProjectCDM.Data.Models
{
    public class TestChoise
    {
        public TestChoise()
        {
            ShowedImages = new List<int>();
        }
        public int StyleId { get; set; }
        public int Count { get; set; }
        [BsonIgnore]
        public List<int> ShowedImages { get; set; }
    }
}
