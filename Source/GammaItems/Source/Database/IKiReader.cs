using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaItems.Source.Database
{
    public interface IKiReader
    {
        public IEnumerable<ItemBase> ReadAllToItemBase(string path);
    }
}
