using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using MediatR;
using Microsoft.Framework.Runtime;
using NHibernate;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class EmployeeByUserNameQueryHandler : IRequestHandler<EmployeeByUserNameQuery, SingleResult<Employee>>
    {
        private readonly IApplicationEnvironment _appEnv;

        public EmployeeByUserNameQueryHandler(IApplicationEnvironment appEnv)
        {
            _appEnv = appEnv;
        }
        public SingleResult<Employee> Handle(EmployeeByUserNameQuery specification)
        {
            //todo: update with interface to access program settings
            var configPath = $"{_appEnv.ApplicationBasePath}\\hibernate.cfg.xml";

            using (ISession session = DataContext.GetTransactedSession(configPath))
            {
                IQuery query = session.CreateQuery("from Employee emp where emp.UserName = :username");
                query.SetParameter("username", specification.UserName);
                var match = query.UniqueResult<Employee>();
                return new SingleResult<Employee>(match);
            }
        }
    }
}