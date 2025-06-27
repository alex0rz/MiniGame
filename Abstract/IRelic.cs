using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstract.Interface;

namespace Abstract
{
    public interface IRelic
    {
        public string Description { get;}

        public IBuff Buff { get; }
    }
}
