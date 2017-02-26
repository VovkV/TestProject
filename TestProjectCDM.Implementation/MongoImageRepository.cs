using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using TestProjectCDM.Data.Interfaces;
using TestProjectCDM.Data.Models;

namespace TestProjectCDM.Implementation
{
    public class MongoImageRepository : IImageRepository
    {
        private IMongoCollection<Style> _collection;
        public MongoImageRepository()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("TestProjectCDM");
            _collection = database.GetCollection<Style>("Styles");
        }

        public bool FillDb(string fullPath, string folderPath)
        {
            
            if (Directory.Exists(fullPath))
            {
                var styleFolders = Directory.GetDirectories(fullPath);
                for (int i = 0; i < styleFolders.Length; i++)
                {
                    var style = new Style();
                    style.Id = i + 1;
                    style.Name = Path.GetFileNameWithoutExtension(styleFolders[i]);
                    var images = Directory.GetFiles(styleFolders[i]);
                    style.Images = new List<Image>(images.Length);
                    for (int j = 0; j < images.Length; j++)
                    {
                        var image = new Image();
                        image.Id = j + 1;
                        image.StyleId = i + 1;
                        image.Link = images[j].Replace(fullPath,"~" + folderPath).Replace(@"\", "/");
                        style.Images.Add(image);
                    }

                    this.UpsertStyle(style);

                }
                return true;
            }
            else
            {
                return false;
            }

        }

        #region IImageRepository
        public Image GetImageById(int styleId, int imageId)
        {
            var style = _collection.Find(p => p.Id == styleId).ToList().First();

            var result = style.Images.Find(x => x.Id == imageId);

            return result;
        }

        public List<Image> GetImagesByStyleId(int id)
        {
            List<Image> result = null;

            var filtered = _collection.Find(p => p.Id == id).ToList();

            result = filtered.First().Images;

            return result;
        }

        public bool UpsertImage(int styleId, Image image)
        {
            try
            {
                var style = _collection.Find(p => p.Id == styleId).ToList().First();
                if (style == null)
                    return false;

                if (style.Images.Exists(x => x.Id == image.Id))
                    style.Images.Find(x => x.Id == image.Id).Link = image.Link;
                else
                    style.Images.Add(image);

                _collection.ReplaceOne(
                    new BsonDocument("_id", styleId),
                    style,
                    new UpdateOptions { IsUpsert = true });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpsertStyle(Style style)
        {
            if (style.Id <= 0 || style.Name == null)
                return false;
            try
            {
                _collection.ReplaceOne(
                    new BsonDocument("_id", style.Id),
                    style,
                    new UpdateOptions { IsUpsert = true });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveImageById(int styleId, int imageId)
        {
            try
            {
                var style = _collection.Find(p => p.Id == styleId).ToList().First();
                if (style == null)
                    return false;
                var result = style.Images.RemoveAll(x => x.Id == imageId);

                _collection.ReplaceOne(
                    new BsonDocument("_id", styleId),
                    style,
                    new UpdateOptions { IsUpsert = true });

                return result == 1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveStyleById(int id)
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

        public List<Style> GetAllStyles()
        {
            var result = _collection.Find(x => true).ToList();

            return result;
        }
        #endregion
    }
}
