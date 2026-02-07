using EnterpriseManagementSystem.Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseManagementSystem.Application.Features.EmployeeFeature.Dtos
{
    public class PagingSearchDto : BasicPagingDto
    {
        public int? Id { get; set; }
        public string? FirstName { get; set; }
    }
}
