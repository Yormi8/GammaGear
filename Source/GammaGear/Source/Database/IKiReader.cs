using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaGear.Source.Database
{
    public interface IKiReader
    {
        public IEnumerable<KiObject> ReadAllToKiObject(string path);
    }
}
