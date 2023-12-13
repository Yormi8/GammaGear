using System.Numerics;
using GammaItems.Source.Database;

namespace GammaItems
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

        public static void MergeAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, Dictionary<TKey, TValue> other) where TValue : IAdditionOperators<TValue, TValue, TValue>
        {
            foreach (var item in other)
            {
                dict.AddOrIncrement(item.Key, item.Value);
            }
        }

        public static Dictionary<School, int> GetDictionaryFromCanonical(this Item item, Canonical canonical)
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
