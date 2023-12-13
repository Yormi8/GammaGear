using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaItems.Source.Database
{
    public abstract class KiWriter : IKiWriter
    {
        public abstract bool Write(string path, IEnumerable<KiObject> values);
        public abstract bool Write(string path, IEnumerable<KiObject> values, bool append);

    }
}
