using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaGear.Source.Database
{
    interface IKiWriter
    {
        public bool Write(string path, IEnumerable<KiObject> values);
        public bool Write(string path, IEnumerable<KiObject> values, bool append);
    }
}
