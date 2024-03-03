using Benday.Presidents.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Benday.Presidents.Api.Models
{
    public abstract class Int32Identity : IInt32Identity
    {
        public int Id { get; set; }
    }
}
