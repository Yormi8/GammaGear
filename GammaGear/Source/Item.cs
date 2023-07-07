using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Windows.Media;
using System.Windows.Navigation;

namespace GammaGear.Source
{
    public abstract class KiObject : object
    {
        public Guid Id { get; set; }
        public Guid KiId { get; set; }
    }
    public class ItemSetBonus : KiObject
    {
        public string SetName { get; set; }
        public List<Item> Bonuses { get; set; }

        public ItemSetBonus()
        {
            Id = Guid.NewGuid();
            KiId = Guid.Empty;
            Bonuses = new List<Item>();
        }

        public override bool Equals(object obj) => Equals(obj as ItemSetBonus);
        public bool Equals(ItemSetBonus other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            return
                SetName == other.SetName &&
                Bonuses.Count == other.Bonuses.Count &&
                Enumerable.SequenceEqual(Bonuses, other.Bonuses);
        }
        public override int GetHashCode() => Id.GetHashCode();
    }
    public class Item : KiObject
    {
        public enum School
        {
            Universal,
            Any = Universal,
            Fire,
            Ice,
            Storm,
            Myth,
            Life,
            Death,
            Balance,
            Shadow,
            Sun,
            Star,
            Moon,
            None
        }
        public enum ItemType
        {
            Hat,
            Robe,
            Shoes,
            Boots = Shoes,
            Weapon,
            Wand = Weapon,
            Athame,
            Amulet,
            Ring,
            Deck,
            Pet,
            Mount,
            TearJewel,
            CircleJewel,
            SquareJewel,
            TriangleJewel,
            PinSquarePip,
            PinSquarePower = PinSquarePip,
            PowerPin = PinSquarePower,
            PinSquareShield,
            ShieldPin = PinSquareShield,
            PinSquareSword,
            SwordPin = PinSquareSword,
            ItemSetBonusData,
            Other,
            None,
        }
        public enum ArenaRank
        {
            Private,
            Sergeant,
            Veteran,
            Knight,
            Captain,
            Commander,
            Warlord
        }
        [Flags]
        public enum ItemFlags : ushort
        {
            FLAG_NoAuction      = 0b_0000_0000_0000_0001,
            FLAG_NoBargain      = 0b_0000_0000_0000_0010,
            FLAG_NoSell         = FLAG_NoBargain,
            FLAG_NoTrade        = 0b_0000_0000_0000_0100,
            FLAG_Retired        = 0b_0000_0000_0000_1000,
            FLAG_CrownsOnly     = 0b_0000_0000_0001_0000,
            FLAG_NoDrop         = 0b_0000_0000_0010_0000,
            FLAG_NoGift         = 0b_0000_0000_0100_0000,
            FLAG_Crafted        = 0b_0000_0000_1000_0000,
            FLAG_PVPOnly        = 0b_0000_0001_0000_0000,
            FLAG_NoPVP          = 0b_0000_0010_0000_0000,
            FLAG_DevItem        = 0b_0000_0100_0000_0000,
        }
        public string Name { get; set; }
        public Guid KiSetBonusID { get; set; }
        public ItemType Type { get; set; } = ItemType.None;
        public int LevelRequirement { get; set; } = 1;
        public ItemFlags Flags { get; set; } = 0;
        public ArenaRank PvpRankRequirement { get; set; } = ArenaRank.Private;
        public ArenaRank PetRankRequirement { get; set; } = ArenaRank.Private;
        public School SchoolRequirement { get; set; } = School.Any;
        public School SchoolRestriction { get; set; } = School.Any;
        public bool Retired { get; set; } = false;
        public int MaxHealth { get; set; } = 0;
        public int MaxMana { get; set; } = 0;
        public int MaxEnergy { get; set; } = 0;
        public int SpeedBonus { get; set; } = 0;
        public int PowerpipChance { get; set; } = 0;
        public int ShadowpipRating { get; set; } = 0;
        public int StunResistChance { get; set; } = 0;
        public int FishingLuck { get; set; } = 0;
        public int ArchmasteryRating { get; set; } = 0;
        public int IncomingHealing { get; set; } = 0;
        public int OutgoingHealing { get; set; } = 0;
        public int PipsGiven { get; set; } = 0;
        public int PowerpipsGiven { get; set; } = 0;
        public School AltSchoolMastery { get; set; }
        public int TearJewelSlots { get; set; } = 0;
        public int CircleJewelSlots { get; set; } = 0;
        public int SquareJewelSlots { get; set; } = 0;
        public int TriangleJewelSlots { get; set; } = 0;
        public int PowerPinSlots { get; set; } = 0;
        public int ShieldPinSlots { get; set; } = 0;
        public int SwordPinSlots { get; set; } = 0;
        public int SetBonusLevel { get; set; } = -1;
        public ItemSetBonus SetBonus { get; set; }
        public Dictionary<School, int> Accuracies { get; set; }
        public Dictionary<School, int> Damages { get; set; }
        public Dictionary<School, int> Resists { get; set; }
        public Dictionary<School, int> Criticals { get; set; }
        public Dictionary<School, int> Blocks { get; set; }
        public Dictionary<School, int> Pierces { get; set; }
        public Dictionary<School, int> FlatDamages { get; set; }
        public Dictionary<School, int> FlatResists { get; set; }
        public Dictionary<School, int> PipConversions { get; set; }
        public Dictionary<string, int> ItemCards { get; set; }

        public Item(Guid ID)
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

            Id = ID;
            KiId = Guid.Empty;
            KiSetBonusID = Guid.Empty;
        }
        public Item() :
            this(Guid.NewGuid()) { }

        public override bool Equals(object obj) => Equals(obj as Item);

        public bool Equals(Item other)
        {
            // Common cases optimization
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            bool equal = ((Name == other.Name) &&
                (Type == other.Type) &&
                (LevelRequirement == other.LevelRequirement) &&
                (Flags == other.Flags) &&
                (PvpRankRequirement == other.PvpRankRequirement) &&
                (PetRankRequirement == other.PetRankRequirement) &&
                (SchoolRequirement == other.SchoolRequirement) &&
                (SchoolRestriction == other.SchoolRestriction) &&
                (Retired == other.Retired) &&
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
                (AltSchoolMastery == other.AltSchoolMastery) &&
                (TearJewelSlots == other.TearJewelSlots) &&
                (CircleJewelSlots == other.CircleJewelSlots) &&
                (SquareJewelSlots == other.SquareJewelSlots) &&
                (TriangleJewelSlots == other.TriangleJewelSlots) &&
                (PowerPinSlots == other.PowerPinSlots) &&
                (ShieldPinSlots == other.ShieldPinSlots) &&
                (SwordPinSlots == other.SwordPinSlots) &&
                (SetBonusLevel == other.SetBonusLevel) &&
                (Accuracies.Count == other.Accuracies.Count && !Accuracies.Except(other.Accuracies).Any()) &&
                (Damages.Count == other.Damages.Count && !Damages.Except(other.Damages).Any()) &&
                (Resists.Count == other.Resists.Count && !Resists.Except(other.Resists).Any()) &&
                (Criticals.Count == other.Criticals.Count && !Criticals.Except(other.Criticals).Any()) &&
                (Blocks.Count == other.Blocks.Count && !Blocks.Except(other.Blocks).Any()) &&
                (Pierces.Count == other.Pierces.Count && !Pierces.Except(other.Pierces).Any()) &&
                (FlatDamages.Count == other.FlatDamages.Count && !FlatDamages.Except(other.FlatDamages).Any()) &&
                (FlatResists.Count == other.FlatResists.Count && !FlatResists.Except(other.FlatResists).Any()) &&
                (PipConversions.Count == other.PipConversions.Count && !PipConversions.Except(other.PipConversions).Any()) &&
                (ItemCards.Count == other.ItemCards.Count && !ItemCards.Except(other.ItemCards).Any())) &&
                // If either setbonus is null then these item are not equal
                ((SetBonus == null ^ other.SetBonus == null) ? false :
                // If both of them are null, then the setbonuses are equal
                ((SetBonus == null && other.SetBonus == null) ? true :
                // If both are not null, check if they are equal
                (SetBonus.SetName == other.SetBonus.SetName &&
                 SetBonus.Bonuses.Count == other.SetBonus.Bonuses.Count)));
                // Checking for if each item is equal in the list causes a recursive action
            return equal;
        }
        public override int GetHashCode() => Id.GetHashCode();
    }
}
