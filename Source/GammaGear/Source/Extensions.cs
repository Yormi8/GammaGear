using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GammaGear.Source;
using GammaGear.Source.Database;

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

        public static Dictionary<Item.School, int> GetDictionaryFromCanonical(this Item item, Canonical canonical)
        {
            return canonical switch
            {
                Canonical.Accuracy => item.Accuracies,
                Canonical.ArmorPiercing => item.Pierces,
                Canonical.Block => item.Blocks,
                Canonical.CriticalHit => item.Criticals,
                Canonical.Damage => item.Damages,
                Canonical.FlatDamage => item.FlatDamages,
                Canonical.ReduceDamage => item.Resists,
                Canonical.FlatReduceDamage => item.FlatResists,
                Canonical.PipConversion => item.PipConversions,
                _ => null,
            };
        }
    }
}
