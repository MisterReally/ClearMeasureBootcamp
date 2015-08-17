using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using MediatR;
using NHibernate;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class EmployeeByUserNameQueryHandler : IRequestHandler<EmployeeByUserNameQuery, SingleResult<Employee>>
    {
        public SingleResult<Employee> Handle(EmployeeByUserNameQuery specification)
        {
            //todo: refactor transacted session
            using (ISession session = DataContext.GetTransactedSession())
            {
                IQuery query = session.CreateQuery("from Employee emp where emp.UserName = :username");
                query.SetParameter("username", specification.UserName);
                var match = query.UniqueResult<Employee>();
                return new SingleResult<Employee>(match);
            }
        }
    }
}