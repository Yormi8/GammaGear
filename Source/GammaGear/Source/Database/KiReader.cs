using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaGear.Source.Database
{
    public abstract class KiReader : IKiReader
    {
        public abstract IEnumerable<KiObject> ReadAllToKiObject(string path);
    }
}
