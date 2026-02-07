using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseManagementSystem.Application.Common.Dtos
{
    public class BasicPagingDto : BasicDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalCount { get; set; }
    }
}
