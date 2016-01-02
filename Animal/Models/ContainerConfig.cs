using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Animal.Controllers;
using Animal.Models.Repository;
using Autofac;
using Autofac.Integration.Mvc;

namespace Animal.Models
{
    public class ContainerConfig
    {
        public static void RegisterContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<SqlDataContext>().As<IDataContext>();
            builder.RegisterType<SqlDataContext>().As<IDataContext>();
            builder.RegisterType<HomeController>().As<HomeController>();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}