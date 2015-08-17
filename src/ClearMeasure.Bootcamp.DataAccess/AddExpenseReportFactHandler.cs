using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportAnalytics;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using MediatR;
using Microsoft.Framework.Runtime;
using NHibernate;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class AddExpenseReportFactHandler : IRequestHandler<AddExpenseReportFactCommand, AddExpenseReportFactResult>
    {
        private readonly IApplicationEnvironment _appEnv;

        public AddExpenseReportFactHandler(IApplicationEnvironment appEnv)
        {
            _appEnv = appEnv;
        }

        public AddExpenseReportFactResult Handle(AddExpenseReportFactCommand command)
        {
            //todo: update with interface to access program settings
            var configPath = $"{_appEnv.ApplicationBasePath}\\hibernate.cfg.xml";

            using (ISession session = DataContext.GetTransactedSession(configPath))
            {
                session.Save(command.ExpenseReportFact);
                session.Transaction.Commit();
            }

            return new AddExpenseReportFactResult
            {
            };
        }
    }
}