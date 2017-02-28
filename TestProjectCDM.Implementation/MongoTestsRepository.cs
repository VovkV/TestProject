using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using TestProjectCDM.Data.Interfaces;
using TestProjectCDM.Data.Models;

namespace TestProjectCDM.Implementation
{
    public class MongoTestsRepository : ITestsRepository
    {
        private IMongoCollection<Test> _collection;
        public MongoTestsRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("TestProjectCDM");
            _collection = database.GetCollection<Test>("Tests");
        }

        #region ITestsRepository
        public List<Test> GetAllTests()
        {
            var result = _collection.Find(x => true).ToList();

            return result;
        }

        public bool AddTest(Test test)
        {
            try
            {
                _collection.InsertOne(test);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public bool RemoveTestById(int id)
        {
            try
            {
                var result = _collection.DeleteOne(new BsonDocument("_id", id));
                return result.DeletedCount == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
