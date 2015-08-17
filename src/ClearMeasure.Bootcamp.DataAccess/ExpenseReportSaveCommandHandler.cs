using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using MediatR;
using Microsoft.Framework.Runtime;
using NHibernate;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class ExpenseReportSaveCommandHandler : IRequestHandler<ExpenseReportSaveCommand, SingleResult<ExpenseReport>>
    {
        private readonly IApplicationEnvironment _appEnv;

        public ExpenseReportSaveCommandHandler(IApplicationEnvironment appEnv)
        {
            _appEnv = appEnv;
        }

        public SingleResult<ExpenseReport> Handle(ExpenseReportSaveCommand request)
        {
            //todo: update with interface to access program settings
            var configPath = $"{_appEnv.ApplicationBasePath}\\hibernate.cfg.xml";

            using (ISession session = DataContext.GetTransactedSession(configPath))
            {
                ITransaction transaction = session.BeginTransaction();
                session.SaveOrUpdate(request.ExpenseReport);
                transaction.Commit();
            }

            return new SingleResult<ExpenseReport>(request.ExpenseReport);
        }
    }
}