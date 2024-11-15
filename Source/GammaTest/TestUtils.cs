using GammaGear;
using GammaItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GammaTest
{
    public class TestUtils
    {
        public static string GetCurrentDirectory([CallerFilePath] string path = "")
        {
            return Path.GetDirectoryName(path) ?? "";
        }

        public static string GetErrorString(ItemBase a, ItemBase b)
        {
            if (a.Equals(b)) return "Items are Equal";
            if (a.GetType() != b.GetType()) return "Items are not the same type";

            string v = "Items are not equal, properties are listed below...\n";

            foreach (PropertyInfo prop in a.GetType().GetProperties())
            {
                if (prop.GetValue(a) is Dictionary<School, int>)
                {
                    v += $"\t{prop.Name}: {string.Join(',', prop.GetValue(a) as Dictionary<School, int> ?? new Dictionary<School, int>())} | {string.Join(',', prop.GetValue(b) as Dictionary<School, int> ?? new Dictionary<School, int>())}\n";
                }
                else if (prop.GetValue(a) is Dictionary<string, int>)
                {
                    v += $"\t{prop.Name}: {string.Join(',', prop.GetValue(a) as Dictionary<string, int> ?? new Dictionary<string, int>())} | {string.Join(',', prop.GetValue(b) as Dictionary<string, int> ?? new Dictionary<string, int>())}\n";
                }
                else if (prop.GetValue(a) is List<Item>)
                {
                    v += $"\t{prop.Name}:\n";
                    List<Item> aList = prop.GetValue(a) as List<Item> ?? new List<Item>();
                    List<Item> bList = prop.GetValue(b) as List<Item> ?? new List<Item>();
                    for (int i = 0; i < aList.Count || i < bList.Count; i++)
                    {
                        v += GetErrorString(aList.ElementAtOrDefault(i) ?? new Item(), bList.ElementAtOrDefault(i) ?? new Item());
                    }
                }
                else
                {
                    v += $"\t{prop.Name}: {prop.GetValue(a)} | {prop.GetValue(b)}\n";
                }
            }

            return v;
        }
    }
}
