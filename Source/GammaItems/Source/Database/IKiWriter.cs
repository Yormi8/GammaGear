using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaItems.Source.Database
{
    interface IKiWriter
    {
        public bool Write(string path, IEnumerable<ItemBase> values);
        public bool Write(string path, IEnumerable<ItemBase> values, bool append);
    }
}
