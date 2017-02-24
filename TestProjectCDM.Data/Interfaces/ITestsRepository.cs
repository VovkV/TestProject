using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProjectCDM.Data.Models;

namespace TestProjectCDM.Data.Interfaces
{
    public interface ITestsRepository
    {
        List<Test> GetAllTests();
        bool AddTest(Test test);
        bool RemoveTestById(int id);
    }
}
