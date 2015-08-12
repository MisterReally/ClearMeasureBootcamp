using Autofac;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClearMeasure.Bootcamp.Dnx.DependencyInjection.Modules
{
    public class CoreModule : Module
    {
        protected override void Load(Autofac.ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(Employee).Assembly)
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(DataContext).Assembly)
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces();
        }


    }
}
