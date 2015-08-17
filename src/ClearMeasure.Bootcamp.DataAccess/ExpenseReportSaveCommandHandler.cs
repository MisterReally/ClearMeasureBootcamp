using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using MediatR;
using NHibernate;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class ExpenseReportSaveCommandHandler : IRequestHandler<ExpenseReportSaveCommand, SingleResult<ExpenseReport>>
    {
        public SingleResult<ExpenseReport> Handle(ExpenseReportSaveCommand request)
        {
            //todo: refactor transacted session
            using (ISession session = DataContext.GetTransactedSession())
            {
                ITransaction transaction = session.BeginTransaction();
                session.SaveOrUpdate(request.ExpenseReport);
                transaction.Commit();
            }

            return new SingleResult<ExpenseReport>(request.ExpenseReport);
        }
    }
}