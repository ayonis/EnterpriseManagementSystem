using EnterpriseManagementSystem.Application.Features.EmployeeFeature.Dtos;
using EnterpriseManagementSystem.Domain.Entities;
using System.Linq.Expressions;

namespace EnterpriseManagementSystem.Application.Features.EmployeeFeature.Filters
{
    public class EmployeeFilterBuilder
    {
        public static List<Expression<Func<Employee, bool>>> Build(PagingSearchDto model)
        {
            var filters = new List<Expression<Func<Employee, bool>>>();
            var fromDate = model.FromDate?.ToDateTime();
            var toDate = model.ToDate?.ToDateTime();


            if (!string.IsNullOrEmpty(model.FirstName))
            {
                filters.Add(u => u.FirstName == model.FirstName);
            }
            if (fromDate != null)
            {
                filters.Add(u => u.CreatedAt >= fromDate);
            }

            if (toDate != null)
            {

                filters.Add(u => u.CreatedAt <= toDate);
            }


            return filters;
        }

    }
}
