using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationModule.Domain.Common
{
    public abstract class BaseEntity<TKey>
    {
        public TKey Id { get; set; } = default!;
    }
}
