﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProjectCDM.Data.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime CompleteTime { get; set; }
        public List<TestChoise> TestChoises { get; set; }
    }
}