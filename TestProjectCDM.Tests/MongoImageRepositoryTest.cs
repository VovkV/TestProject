using System;
using System.Net.Mime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestProjectCDM.Data.Interfaces;
using TestProjectCDM.Data.Models;
using TestProjectCDM.Implementation;

namespace TestProjectCDM.Tests
{
    [TestClass]
    public class MongoImageRepositoryTest
    {
        private Random _rnd;
        private string _testText;
        private IImageRepository _repo;
        private int _min;
        private int _max;

        public MongoImageRepositoryTest()
        {
            _rnd = new Random();
            _testText = "UnitTest";
            _repo = new MongoImageRepository();
            _min = 10000;
            _max = 20000;
        }

        [TestMethod]
        public void UpsertDeleteStyle()
        {
            var id = _rnd.Next(_min, _max);
            var newName = "Second Upsert";
            var style  = new Style()
            {
                Id = id,
                Name = _testText
            };

            var resultNull = _repo.GetStyleById(id);

            _repo.UpsertStyle(style);

            var resultStyle = _repo.GetStyleById(id);

            style.Name = newName;

            _repo.UpsertStyle(style);

            var resultSecondStyle = _repo.GetStyleById(id);

            var resultDelete = _repo.RemoveStyleById(id);
            var resultSecondDelete = _repo.RemoveStyleById(id);

            Assert.IsNull(resultNull);
            Assert.AreEqual(_testText,resultStyle.Name);
            Assert.AreEqual(newName,resultSecondStyle.Name);
            Assert.AreEqual(true,resultDelete);
            Assert.AreEqual(false,resultSecondDelete);
        }

        [TestMethod]
        public void UpsertDeleteImage()
        {
            var id = _rnd.Next(_min, _max);
            var styleId = 1;
            var newLink = "Second Upsert";
            var image = new Image()
            {
                Id = id,
                StyleId = id,
                Link = _testText
            };

            var resultNull = _repo.GetImageById(id,id);


            var resultFalse = _repo.UpsertImage(id,image);

            var resultSecondNull = _repo.GetImageById(id,id);

            image.StyleId = styleId;
            image.Link = newLink;

            var resultTrue = _repo.UpsertImage(styleId,image);

            var resultImage = _repo.GetImageById(styleId,image.Id);

            var resultDelete = _repo.RemoveImageById(styleId, image.Id);
            var resultSecondDelete = _repo.RemoveImageById(styleId, image.Id);

            Assert.IsNull(resultNull);
            Assert.AreEqual(false, resultFalse);
            Assert.IsNull(resultSecondNull);
            Assert.AreEqual(true, resultTrue);
            Assert.AreEqual(newLink,resultImage.Link);
            Assert.AreEqual(true,resultDelete);
            Assert.AreEqual(false, resultSecondDelete);

        }
    }
}
