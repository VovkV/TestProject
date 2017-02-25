using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace TestProjectCDM.Data.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Link { get; set; }

        [BsonIgnore]
        public int StyleId { get; set; }
    }
}
