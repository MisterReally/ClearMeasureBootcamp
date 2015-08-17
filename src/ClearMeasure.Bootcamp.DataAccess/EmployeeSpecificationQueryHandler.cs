using System;
using System.Collections.Generic;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using NHibernate;
using MediatR;
using Microsoft.Framework.Runtime;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class EmployeeSpecificationQueryHandler : IRequestHandler<EmployeeSpecificationQuery, MultipleResult<Employee>>
    {
        private readonly IApplicationEnvironment _appEnv;

        public EmployeeSpecificationQueryHandler(IApplicationEnvironment appEnv)
        {
            _appEnv = appEnv;
        }

        public MultipleResult<Employee> Handle(EmployeeSpecificationQuery request)
        {
            //todo: update with interface to access program settings
            var configPath = $"{_appEnv.ApplicationBasePath}\\hibernate.cfg.xml";

            using (var session = DataContext.GetTransactedSession(configPath))
            {
                var criteria = session.CreateCriteria(typeof(Employee));
                criteria.SetCacheable(true);
                
                var list = criteria.List<Employee>();
                var employees = new List<Employee>(list).ToArray();
                Array.Sort(employees);
                return new MultipleResult<Employee> {Results = employees};
            }
        }
    }
}