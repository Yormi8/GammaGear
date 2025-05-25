using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Windows.UI.WebUI;

namespace GammaGear.Models.DML
{
    public class PropertyFormat : Object
    {
        public string Type { get; init; }
        public string Name { get; init; }

        public PropertyFormat(string type, string name)
        {
            Type = type;
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name}<{Type}>";
        }
    }
}
