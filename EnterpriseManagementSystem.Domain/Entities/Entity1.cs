using EnterpriseManagementSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseManagementSystem.Domain.Entities
{
    public class Entity1 : FullAuditableEntity<int>
    {
        public virtual ICollection<Entity2> Entity2s { get; set; } = new HashSet<Entity2>();
    }
}
