using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;

namespace WebLibrary.App_Start
{
    public static class UnityConfig
    {
        public static void RegisterTypes()  //регистрир. маппинг резолвера
        {
            var container = new UnityContainer();

            container.RegisterInstance<IUnityContainer>(container); //method UnityConfig
            AutoMapperConfig.RegisterWithUnity(container);  //method AutoMapperConfig

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            //// внедрение зависимостей2
            ///а)сначала надо созд. класс NinjectRegistrations : NinjectModule
            
    //namespace Weblibrary.Utils //там где MyHelper
    //{
    //public class NinjectRegistrations : NinjectModule
    //    {
    //        public override void Load()
    //        {
    //            Bind<IRepository>().To<BookRepository>();
    //        }
    //    }
    //}
    //b)
    //NinjectModule registrations = new NinjectRegistrations();
    //var kernel = new StandardKernel(registrations);
    //DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));

}
    }
}