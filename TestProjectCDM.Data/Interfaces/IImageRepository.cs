using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProjectCDM.Data.Models;

namespace TestProjectCDM.Data.Interfaces
{
    public interface IImageRepository
    {
        List<Style> GetAllStyles();
        Image GetImageById(int styleId, int imageId);
        List<Image> GetImagesByStyleId(int id);
        bool UpsertImage(int styleId, Image image);
        bool UpsertStyle(Style style);
        bool RemoveImageById(int styleId, int imageId);
        bool RemoveStyleById(int id);
    }
}
