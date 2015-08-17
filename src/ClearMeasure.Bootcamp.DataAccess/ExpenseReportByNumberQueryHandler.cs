using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using MediatR;
using Microsoft.Framework.Runtime;
using NHibernate;
using NHibernate.Criterion;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class ExpenseReportByNumberQueryHandler : IRequestHandler<ExpenseReportByNumberQuery, SingleResult<ExpenseReport>>
    {
        private readonly IApplicationEnvironment _appEnv;

        public ExpenseReportByNumberQueryHandler(IApplicationEnvironment appEnv)
        {
            _appEnv = appEnv;
        }

        public SingleResult<ExpenseReport> Handle(ExpenseReportByNumberQuery request)
        {
            //todo: update with interface to access program settings
            var configPath = $"{_appEnv.ApplicationBasePath}\\hibernate.cfg.xml";

            using (ISession session = DataContext.GetTransactedSession(configPath))
            {
                ICriteria criteria = session.CreateCriteria(typeof (ExpenseReport));
                criteria.Add(Restrictions.Eq("Number", request.ExpenseReportNumber));
                criteria.SetFetchMode("AuditEntries", FetchMode.Eager);
                var result = criteria.UniqueResult<ExpenseReport>();
                return new SingleResult<ExpenseReport>(result);
            }
        }
    }
}