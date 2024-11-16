using GammaGear.Extensions;
using GammaItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaGear.ViewModels
{
    public class ItemViewModel : ViewModelBase
    {
        private readonly Item _item;

        public string Name => _item.Name;
        public ItemType Type => _item.Type;
        public int LevelRequirement => _item.LevelRequirement;
        public ItemFlags Flags => _item.Flags;
        public ArenaRank PvpRankRequirement => _item.PvpRankRequirement;
        public ArenaRank PetRankRequirement => _item.PetRankRequirement;
        public School SchoolRequirement => _item.SchoolRequirement;
        public School SchoolRestriction => _item.SchoolRestriction;
        public bool IsRetired => _item.IsRetired;
        public bool IsDebug => _item.IsDebug;
        public bool IsCrownsOnly => (_item.Flags & ItemFlags.FLAG_CrownsOnly) == ItemFlags.FLAG_CrownsOnly;
        public bool IsPVPOnly => (_item.Flags & ItemFlags.FLAG_PVPOnly) == ItemFlags.FLAG_PVPOnly;
        public bool IsFavorite => false;
        public int MaxHealth => _item.MaxHealth;
        public int MaxMana => _item.MaxMana;
        public int MaxEnergy => _item.MaxEnergy;
        public int SpeedBonus => _item.SpeedBonus;
        public int PowerpipChance => _item.PowerpipChance;
        public int ShadowpipRating => _item.ShadowpipRating;
        public int StunResistChance => _item.StunResistChance;
        public int FishingLuck => _item.FishingLuck;
        public int ArchmasteryRating => _item.ArchmasteryRating;
        public int IncomingHealing => _item.IncomingHealing;
        public int OutgoingHealing => _item.OutgoingHealing;
        public int PipsGiven => _item.PipsGiven;
        public int PowerpipsGiven => _item.PowerpipsGiven;
        public int TearJewelSlots => _item.TearJewelSlots;
        public int CircleJewelSlots => _item.CircleJewelSlots;
        public int SquareJewelSlots => _item.SquareJewelSlots;
        public int TriangleJewelSlots => _item.TriangleJewelSlots;
        public int PowerPinSlots => _item.PowerPinSlots;
        public int ShieldPinSlots => _item.ShieldPinSlots;
        public int SwordPinSlots => _item.SwordPinSlots;
        public ItemSet SetBonus => _item.SetBonus;
        public IReadOnlyDictionary<School, int> Accuracies => _item.Accuracies;
        public IReadOnlyDictionary<School, int> Damages => _item.Damages;
        public IReadOnlyDictionary<School, int> Resists => _item.Resists;
        public IReadOnlyDictionary<School, int> Criticals => _item.Criticals;
        public IReadOnlyDictionary<School, int> Blocks => _item.Blocks;
        public IReadOnlyDictionary<School, int> Pierces => _item.Pierces;
        public IReadOnlyDictionary<School, int> FlatDamages => _item.FlatDamages;
        public IReadOnlyDictionary<School, int> FlatResists => _item.FlatResists;
        public IReadOnlyDictionary<School, int> PipConversions => _item.PipConversions;
        public IReadOnlyDictionary<string, int> ItemCards => _item.ItemCards;
        public IReadOnlyCollection<School> AltSchoolMasteries => _item.AltSchoolMasteries;

        public Uri SchoolIcon => _item.SchoolRestriction != School.None ?
                                    _item.SchoolRequirement.ToIconUri() :
                                    _item.SchoolRestriction.ToSlashedIconUri();

        public Uri EquipmentTypeIcon => _item.Type.ToIconUri();

        public ItemViewModel(Item item)
        {
            _item = item;
        }
    }
}
