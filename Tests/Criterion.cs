using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public abstract class Criterion
    {
        public virtual bool HypoTest(double[] Selection, DistributionFunctions.Distribution distribution)
        { return false; }
    }
}
