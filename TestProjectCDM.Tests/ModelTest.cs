using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestProjectCDM.Data.Models;

namespace TestProjectCDM.Tests
{
    [TestClass]
    public class ModelTest
    {
        private Random _rnd;

        public ModelTest()
        {
            _rnd = new Random();
        }

        [TestMethod]
        public void TestClassTestGetWinners()
        {
            var test = new Test();
            for (int i = 0; i < 10; i++)
            {
                test.Steps.Add(new TestStep()
                {
                    ChosenStyleId = i
                });
            }

            var resultList = test.GetWinnersId();

            Assert.AreEqual(10,resultList.Count);
        }
        [TestMethod]
        public void TestClassTestGetOneWinner()
        {
            var test = new Test();
            for (int i = 0; i < 10; i++)
            {
                test.Steps.Add(new TestStep()
                {
                    ChosenStyleId = i
                });
            }
            test.Steps.Add(new TestStep()
            {
                ChosenStyleId = 0
            });
            var resultList = test.GetWinnersId();

            Assert.AreEqual(1, resultList.Count);
            Assert.AreEqual(0,resultList.First());
        }
    }
}
