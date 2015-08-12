using ClearMeasure.Bootcamp.Core.Model;
using MediatR;

namespace ClearMeasure.Bootcamp.Core.Plugins.DataAccess
{
    public class ExpenseReportByNumberQuery : IRequest<SingleResult<ExpenseReport>>
    {
        public string ExpenseReportNumber { get; set; }
    }
}