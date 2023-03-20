using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using W101ToolUI.Source;

namespace GammaGear.Source
{
    public static class Extensions
    {
        public static bool AddOrIncrement<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value) where TValue : IAdditionOperators<TValue, TValue, TValue>
        {
            bool added = dict.TryAdd(key, value);
            if (!added)
            {
                dict[key] += value;
            }
            return added;
        }

        public static Dictionary<Item.School, int> GetDictionaryFromCanonical(this Item item, XmlToDb.Canonical canonical)
        {
            return canonical switch
            {
                XmlToDb.Canonical.Accuracy => item.Accuracies,
                XmlToDb.Canonical.ArmorPiercing => item.Pierces,
                XmlToDb.Canonical.Block => item.Blocks,
                XmlToDb.Canonical.CriticalHit => item.Criticals,
                XmlToDb.Canonical.Damage => item.Damages,
                XmlToDb.Canonical.FlatDamage => item.FlatDamages,
                XmlToDb.Canonical.ReduceDamage => item.Resists,
                XmlToDb.Canonical.FlatReduceDamage => item.FlatResists,
                XmlToDb.Canonical.PipConversion => item.PipConversions,
                _ => null,
            };
        }
    }
}
