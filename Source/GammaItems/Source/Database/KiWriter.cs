using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaItems.Source.Database
{
    public abstract class KiWriter : IKiWriter
    {
        public abstract bool Write(string path, IEnumerable<ItemBase> values);
        public abstract bool Write(string path, IEnumerable<ItemBase> values, bool append);

    }
}
