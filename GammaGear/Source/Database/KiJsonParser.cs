using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaGear.Source.Database
{
    class KiJsonParser<T> : KiParser<T>
        where T : KiLocaleBank, new()
    {
        public KiJsonParser(string localePath) : base(localePath)
        {
        }
        public override PropertyClass ReadToPropertyClass(string path)
        {
            throw new NotImplementedException();
        }
    }
}
