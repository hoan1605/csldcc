using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCCVC.Infrastructure.Domain
{
    public abstract class EntityBase
    {
        public virtual long Id { get; set; }
    }

}
