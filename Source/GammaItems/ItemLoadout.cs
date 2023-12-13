using System.Runtime.CompilerServices;

namespace GammaItems
{
    public class ItemLoadout
    {
        public string Name;
        public string Description;
        public Uri IconUri;
        public List<Item> Items;
        public string WizardName;
        public int WizardLevel = 1;
        public School WizardSchool = School.Fire;
        public ArenaRank WizardPvpRank = ArenaRank.Private;
        public ArenaRank WizardPetRank = ArenaRank.Private;

        public bool IsLegal => CalculateLegality().Any();
        public bool ContainsRetired => Items.Any(x => x.IsRetired);
        public bool ContainsDebug => Items.Any(x => x.IsDebug);

        public ItemLoadout()
        {
            Items = new List<Item>();
        }

        public Item CalculateStats()
        {
            Dictionary<ItemSet, int> itemSetDict = CalculateItemSetContribution();
            List<Item> fullItems = new List<Item>(Items);

            // Add the set bonuses to the list of items to calculate from
            foreach (var itemSetPair in itemSetDict)
            {
                foreach (var item in itemSetPair.Key.Bonuses)
                {
                    if (itemSetPair.Value >= item.SetBonusLevel)
                    {
                        fullItems.Add(item);
                    }
                }
            }

            Item sum = new Item
            {
                Id = Guid.Empty,
                IsCustom = true,
                Name = Name,
                Type = ItemType.Other
            };

            foreach (var item in fullItems)
            {
                sum.LevelRequirement = Math.Max(sum.LevelRequirement, item.LevelRequirement);
                sum.PvpRankRequirement = (ArenaRank)Math.Max((int)sum.PvpRankRequirement, (int)item.PvpRankRequirement);
                sum.PetRankRequirement = (ArenaRank)Math.Max((int)sum.PetRankRequirement, (int)item.PetRankRequirement);
                sum.IsRetired |= item.IsRetired;
                sum.IsDebug |= item.IsDebug;

                sum.MaxHealth += item.MaxHealth;
                sum.MaxMana += item.MaxMana;
                sum.MaxEnergy += item.MaxEnergy;
                sum.SpeedBonus += item.SpeedBonus;
                sum.PowerpipChance += item.PowerpipChance;
                sum.ShadowpipRating += item.ShadowpipRating;
                sum.StunResistChance += item.StunResistChance;
                sum.FishingLuck += item.FishingLuck;
                sum.ArchmasteryRating += item.ArchmasteryRating;
                sum.IncomingHealing += item.IncomingHealing;
                sum.OutgoingHealing += item.OutgoingHealing;
                sum.PipsGiven += item.PipsGiven;
                sum.PowerpipsGiven += item.PowerpipsGiven;
                sum.TearJewelSlots += item.TearJewelSlots;
                sum.CircleJewelSlots += item.CircleJewelSlots;
                sum.SquareJewelSlots += item.SquareJewelSlots;
                sum.TriangleJewelSlots += item.TriangleJewelSlots;
                sum.PowerPinSlots += item.PowerPinSlots;
                sum.ShieldPinSlots += item.ShieldPinSlots;
                sum.SwordPinSlots += item.SwordPinSlots;

                sum.Accuracies.MergeAdd(item.Accuracies);
                sum.Damages.MergeAdd(item.Damages);
                sum.Resists.MergeAdd(item.Resists);
                sum.Criticals.MergeAdd(item.Criticals);
                sum.Blocks.MergeAdd(item.Blocks);
                sum.Pierces.MergeAdd(item.Pierces);
                sum.FlatDamages.MergeAdd(item.FlatDamages);
                sum.FlatResists.MergeAdd(item.FlatResists);
                sum.PipConversions.MergeAdd(item.PipConversions);
                sum.ItemCards.MergeAdd(item.ItemCards);
                sum.AltSchoolMasteries.UnionWith(item.AltSchoolMasteries);
            }

            return sum;
        }

        public List<IllegalitySet> CalculateLegality()
        {
            var invalidItems = new List<IllegalitySet>();
            foreach (var item in Items)
            {
                IllegalitySet entry = new IllegalitySet
                {
                    Item = item
                };

                if (item is ItemSetBonus)
                {
                    continue;
                }

                if (item.LevelRequirement > WizardLevel)
                {
                    entry.Reasons.Add(IllegalityReason.LevelRequirementConflict);
                }

                if (item.SchoolRequirement != School.Any &&
                    item.SchoolRequirement != School.None &&
                    item.SchoolRequirement != WizardSchool)
                {
                    entry.Reasons.Add(IllegalityReason.SchoolRequirementConflict);
                }

                if (item.SchoolRestriction != School.Any &&
                    item.SchoolRestriction != School.None &&
                    item.SchoolRestriction == WizardSchool)
                {
                    entry.Reasons.Add(IllegalityReason.SchoolRestrictionConflict);
                }

                if (item.PvpRankRequirement > WizardPvpRank)
                {
                    entry.Reasons.Add(IllegalityReason.PvpArenaRankRequirementConflict);
                }

                if (item.PetRankRequirement > WizardPetRank)
                {
                    entry.Reasons.Add(IllegalityReason.PetArenaRankRequirementConflict);
                }

                // TODO: Do checks for duplicate types of items, for example having two
                //       hats on is illegal, but we don't check for that here.

                if (entry.Reasons.Any())
                {
                    invalidItems.Add(entry);
                }
            }

            return invalidItems;
        }

        public Dictionary<ItemSet, int> CalculateItemSetContribution()
        {
            return null;
        }
    }
}
