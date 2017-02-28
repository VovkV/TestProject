using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestProjectCDM.Data.Interfaces;
using TestProjectCDM.Implementation;

namespace TestProjectCDM.Tests
{
    [TestClass]
    public class MongoImageRepositoryTest
    {
        [TestMethod]
        public void UpsertStyle()
        {
            Mock<IImageRepository> mock = new Mock<IImageRepository>();
            IImageRepository repository = new MongoImageRepository();
        }
    }
}
