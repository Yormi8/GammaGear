using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace GammaGear.Source.Database
{
    public class KiJsonParser<T> : KiParser<T>
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
