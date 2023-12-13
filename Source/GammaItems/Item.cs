using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GammaItems
{
    public class Item : ItemBase
    {
        public string Name = "";
        public ItemType Type = ItemType.None;
        public int LevelRequirement = 1;
        public ItemFlags Flags = 0;
        public ArenaRank PvpRankRequirement = ArenaRank.Private;
        public ArenaRank PetRankRequirement = ArenaRank.Private;
        public School SchoolRequirement = School.None;
        public School SchoolRestriction = School.None;
        public bool IsRetired = false;
        public bool IsDebug = false;
        public int MaxHealth = 0;
        public int MaxMana = 0;
        public int MaxEnergy = 0;
        public int SpeedBonus = 0;
        public int PowerpipChance = 0;
        public int ShadowpipRating = 0;
        public int StunResistChance = 0;
        public int FishingLuck = 0;
        public int ArchmasteryRating = 0;
        public int IncomingHealing = 0;
        public int OutgoingHealing = 0;
        public int PipsGiven = 0;
        public int PowerpipsGiven = 0;
        public int TearJewelSlots = 0;
        public int CircleJewelSlots = 0;
        public int SquareJewelSlots = 0;
        public int TriangleJewelSlots = 0;
        public int PowerPinSlots = 0;
        public int ShieldPinSlots = 0;
        public int SwordPinSlots = 0;
        public ItemSet SetBonus = null;
        public Dictionary<School, int> Accuracies;
        public Dictionary<School, int> Damages;
        public Dictionary<School, int> Resists;
        public Dictionary<School, int> Criticals;
        public Dictionary<School, int> Blocks;
        public Dictionary<School, int> Pierces;
        public Dictionary<School, int> FlatDamages;
        public Dictionary<School, int> FlatResists;
        public Dictionary<School, int> PipConversions;
        public Dictionary<string, int> ItemCards;
        public SortedSet<School> AltSchoolMasteries;

        public Item(
            Guid Id,
            bool IsCustom) :
                base(Id, IsCustom)
        {
            Accuracies = new Dictionary<School, int>();
            Damages = new Dictionary<School, int>();
            Resists = new Dictionary<School, int>();
            Criticals = new Dictionary<School, int>();
            Blocks = new Dictionary<School, int>();
            Pierces = new Dictionary<School, int>();
            FlatDamages = new Dictionary<School, int>();
            FlatResists = new Dictionary<School, int>();
            PipConversions = new Dictionary<School, int>();
            ItemCards = new Dictionary<string, int>();
            AltSchoolMasteries = new SortedSet<School>();
        }
        public Item() : this(Guid.NewGuid(), true)
        { }

        public override bool Equals(object obj) => Equals(obj as Item);

        public bool Equals(Item other)
        {
            // Common cases optimization
            bool equal = base.Equals(other) &&
                ((Name == other.Name) &&
                (Type == other.Type) &&
                (LevelRequirement == other.LevelRequirement) &&
                (Flags == other.Flags) &&
                (PvpRankRequirement == other.PvpRankRequirement) &&
                (PetRankRequirement == other.PetRankRequirement) &&
                (SchoolRequirement == other.SchoolRequirement) &&
                (SchoolRestriction == other.SchoolRestriction) &&
                (IsRetired == other.IsRetired) &&
                (MaxHealth == other.MaxHealth) &&
                (MaxMana == other.MaxMana) &&
                (MaxEnergy == other.MaxEnergy) &&
                (SpeedBonus == other.SpeedBonus) &&
                (PowerpipChance == other.PowerpipChance) &&
                (ShadowpipRating == other.ShadowpipRating) &&
                (StunResistChance == other.StunResistChance) &&
                (FishingLuck == other.FishingLuck) &&
                (ArchmasteryRating == other.ArchmasteryRating) &&
                (IncomingHealing == other.IncomingHealing) &&
                (OutgoingHealing == other.OutgoingHealing) &&
                (PipsGiven == other.PipsGiven) &&
                (PowerpipsGiven == other.PowerpipsGiven) &&
                (TearJewelSlots == other.TearJewelSlots) &&
                (CircleJewelSlots == other.CircleJewelSlots) &&
                (SquareJewelSlots == other.SquareJewelSlots) &&
                (TriangleJewelSlots == other.TriangleJewelSlots) &&
                (PowerPinSlots == other.PowerPinSlots) &&
                (ShieldPinSlots == other.ShieldPinSlots) &&
                (SwordPinSlots == other.SwordPinSlots) &&
                (Accuracies.Count == other.Accuracies.Count && !Accuracies.Except(other.Accuracies).Any()) &&
                (Damages.Count == other.Damages.Count && !Damages.Except(other.Damages).Any()) &&
                (Resists.Count == other.Resists.Count && !Resists.Except(other.Resists).Any()) &&
                (Criticals.Count == other.Criticals.Count && !Criticals.Except(other.Criticals).Any()) &&
                (Blocks.Count == other.Blocks.Count && !Blocks.Except(other.Blocks).Any()) &&
                (Pierces.Count == other.Pierces.Count && !Pierces.Except(other.Pierces).Any()) &&
                (FlatDamages.Count == other.FlatDamages.Count && !FlatDamages.Except(other.FlatDamages).Any()) &&
                (FlatResists.Count == other.FlatResists.Count && !FlatResists.Except(other.FlatResists).Any()) &&
                (PipConversions.Count == other.PipConversions.Count && !PipConversions.Except(other.PipConversions).Any()) &&
                (ItemCards.Count == other.ItemCards.Count && !ItemCards.Except(other.ItemCards).Any()) &&
                (AltSchoolMasteries.SetEquals(other.AltSchoolMasteries))) &&
                // If either setbonus is null then these item are not equal
                ((SetBonus == null ^ other.SetBonus == null) ? false :
                // If both of them are null, then the setbonuses are equal
                ((SetBonus == null && other.SetBonus == null) ? true :
                // If both are not null, check if they are equal
                (SetBonus?.SetName == other.SetBonus?.SetName &&
                 SetBonus?.Bonuses.Count == other.SetBonus?.Bonuses.Count)));
                // Checking for if each item is equal in the list causes a recursive action
            return equal;
        }
        public override int GetHashCode() => base.GetHashCode();
    }
}
