﻿using Autofac;
using Autofac.Integration.WebApi;
using Sample.Employees.Data;
using Sample.Employees.Domain;
using Sample.Orders.DatabaseAccess;
using Sample.WebApi.Ioc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Sample.WebApi
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Create the container builder.
            var builder = new ContainerBuilder();
            // Register the Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            // Register other dependencies.
            builder.RegisterModule(new DependencyRegistrar());
            // Build the container.
            var container = builder.Build();
            // Create the depenedency resolver.
            var resolver = new AutofacWebApiDependencyResolver(container);
            // Configure Web API with the dependency resolver.
            GlobalConfiguration.Configuration.DependencyResolver = resolver;

            //SEED EMPLOYEE
            var employeeRepo = container.Resolve<IEmployeeRepository>();
            var employee = employeeRepo.GetByNumber(123);
            if (employee == null)
            {
                employee = new Employee("Test", "Employee", 123);
                employeeRepo.Save(employee); 
            }

        }
    }
}