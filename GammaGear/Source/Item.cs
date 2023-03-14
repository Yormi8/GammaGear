using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Windows.Media;

namespace GammaGear.Source
{
    public class Item
    {
        public class ItemSetBonus 
        {
            public Guid ID;
            public Guid KI_ID;
            public string SetName = "";
            public List<Item> Bonuses = new List<Item>();

            public ItemSetBonus() 
            { 
                ID = Guid.NewGuid();
                KI_ID = Guid.Empty;
            }
        }
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
        public enum ItemFlags : byte
        {
            FLAG_NoAuction      = 0b_0000_0001,
            FLAG_NoBargain      = 0b_0000_0010,
            FLAG_NoSell         = FLAG_NoBargain,
            FLAG_NoTrade        = 0b_0000_0100,
            FLAG_Retired        = 0b_0000_1000,
            FLAG_CrownsOnly     = 0b_0001_0000,
            FLAG_NoDrop         = 0b_0010_0000,
            FLAG_NoGift         = 0b_0100_0000,
            FLAG_Crafted        = 0b_1000_0000,
        }
        public string Name { get; set; }
        public Guid ID { get; set; }
        public Guid KI_ID { get; set; }
        public Guid KI_SetBonusID { get; set; }
        public ItemType Type { get; set; } = ItemType.None;
        public int LevelRequirement { get; set; } = 1;
        public ItemFlags Flags { get; set; } = 0;
        public ArenaRank PVPRankRequirement { get; set; } = ArenaRank.Private;
        public ArenaRank PetRankRequirement { get; set; } = ArenaRank.Private;
        public School SchoolRequirement { get; set; } = School.None;
        public School SchoolRestriction { get; set; } = School.None;
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
        public List<KeyValuePair<School, int>> Accuracies { get; set; }
        public List<KeyValuePair<School, int>> Damages { get; set; }
        public List<KeyValuePair<School, int>> Resists { get; set; }
        public List<KeyValuePair<School, int>> Criticals { get; set; }
        public List<KeyValuePair<School, int>> Blocks { get; set; }
        public List<KeyValuePair<School, int>> Pierces { get; set; }
        public List<KeyValuePair<School, int>> FlatDamages { get; set; }
        public List<KeyValuePair<School, int>> FlatResists { get; set; }
        public List<KeyValuePair<School, int>> PipConversions { get; set; }
        public List<KeyValuePair<string, int>> ItemCards { get; set; }

        public Item(Guid ID)
        {
            Accuracies = new List<KeyValuePair<School, int>>();
            Damages = new List<KeyValuePair<School, int>>();
            Resists = new List<KeyValuePair<School, int>>();
            Criticals = new List<KeyValuePair<School, int>>();
            Blocks = new List<KeyValuePair<School, int>>();
            Pierces = new List<KeyValuePair<School, int>>();
            FlatDamages = new List<KeyValuePair<School, int>>();
            FlatResists = new List<KeyValuePair<School, int>>();
            PipConversions = new List<KeyValuePair<School, int>>();
            ItemCards = new List<KeyValuePair<string, int>>();

            this.ID = ID;
            KI_ID = Guid.Empty;
            KI_SetBonusID = Guid.Empty;
        }
        public Item() : 
            this(Guid.NewGuid()) { }

        
    }
}
