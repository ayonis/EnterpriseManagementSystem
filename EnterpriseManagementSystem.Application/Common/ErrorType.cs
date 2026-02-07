using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseManagementSystem.Application.Common
{
    public enum ErrorType
    {
        None = 0,
        Business = 1,
        Validation = 2,
        NotFound = 3,
        Unauthorized = 4,
        Conflict = 5,
        System = 6,
    }
}
