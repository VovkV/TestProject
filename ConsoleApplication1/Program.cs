using System;
using System.Collections.Generic;
using TestProjectCDM.Data.Models;
using TestProjectCDM.Implementation;


namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var rep = new MongoTestsRepository();
            var l = new List<TestChoise> {new TestChoise() {Count = 3, StyleId = 1}};
            var t = new Test {CompleteTime = DateTime.Now, Id = 1,TestChoises = l};

            rep.AddTest(t);
            var res = rep.GetAllTests();
            rep.RemoveTestById(1);
            var rea = rep.GetAllTests();
        }
    }
}
