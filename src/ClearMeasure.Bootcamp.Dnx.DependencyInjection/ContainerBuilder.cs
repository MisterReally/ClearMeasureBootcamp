using Autofac;
using Autofac.Framework.DependencyInjection;
using ClearMeasure.Bootcamp.Dnx.DependencyInjection.Modules;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ClearMeasure.Bootcamp.Dnx.DependencyInjection
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class BootcampContainerBuilder : Autofac.ContainerBuilder
    {
        public BootcampContainerBuilder(IServiceCollection services)
        {
            var foo = Assembly.GetCallingAssembly();
            this.RegisterAssemblyTypes(foo).AsImplementedInterfaces();

            this.RegisterModule(new CoreModule());
            if(services != null)
                this.Populate(services);
        }

        public IServiceProvider ResolveServiceProvider()
        {
            var container = this.Build();
            return container.Resolve<IServiceProvider>();
        }

    }
}
