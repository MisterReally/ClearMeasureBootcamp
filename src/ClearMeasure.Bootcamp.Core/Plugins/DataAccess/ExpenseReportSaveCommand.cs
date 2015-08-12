using ClearMeasure.Bootcamp.Core.Model;
using MediatR;

namespace ClearMeasure.Bootcamp.Core.Plugins.DataAccess
{
    public class ExpenseReportSaveCommand : IRequest<SingleResult<ExpenseReport>>
    {
        public ExpenseReport ExpenseReport { get; set; }
    }
}