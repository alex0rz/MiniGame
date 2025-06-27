using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstract;
using Abstract.Interface;

namespace GameModels.Model.Relic
{
    public class Relic(IBuff buff) : IRelic
    {
        public string Description => Buff.Description;
        public IBuff Buff { get; } = buff;
    }
}
