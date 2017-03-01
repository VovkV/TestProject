using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestProjectCDM.Data.Interfaces;
using TestProjectCDM.Data.Models;
using TestProjectCDM.Implementation;

namespace TestProjectCDM.Tests
{
    [TestClass]
    public class MongoTestsRepositoryTest
    {
        private ITestsRepository _repo;
        public MongoTestsRepositoryTest()
        {
            _repo = new MongoTestsRepository();

        }

        [TestMethod]
        public void AddTestTest()
        {
            var resultBefore = _repo.GetAllTests().Count;
            var id = Guid.NewGuid();
            _repo.AddTest(new Test()
            {
                CompleteTime = DateTime.Now,
                Id = id,
                Username = "Nameless"
            });

            var resultAfter = _repo.GetAllTests().Count;

            Assert.AreEqual(1, resultAfter - resultBefore);

            _repo.RemoveTestByGuid(id);
        }

        [TestMethod]
        public void RemoveTestByGuidTest()
        {
            var resultBefore = _repo.GetAllTests().Count;

            var id = Guid.NewGuid();

            _repo.AddTest(new Test()
            {
                CompleteTime = DateTime.Now,
                Id = id,
                Username = "Nameless"
            });

            _repo.RemoveTestByGuid(id);

            var resultAfter = _repo.GetAllTests().Count;

            Assert.AreEqual(0, resultAfter - resultBefore);
        }
    }
}
