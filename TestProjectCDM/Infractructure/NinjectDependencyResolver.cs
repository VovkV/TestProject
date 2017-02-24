using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using TestProjectCDM.Data.Interfaces;
using TestProjectCDM.Data.Models;
using TestProjectCDM.Implementation;

namespace TestProjectCDM.Infractructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind<IImageRepository>().To<MongoImageRepository>();
            kernel.Bind<ITestsRepository>().To<MongoTestsRepository>();
        }
    }
}