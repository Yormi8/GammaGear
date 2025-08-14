using CommunityToolkit.Mvvm.Input;
using GammaGear.Extensions;
using GammaItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaGear.ViewModels.Controls
{
    public partial class ItemViewModel(Item item) : ViewModelBase
    {
        public string Name => item.Name;
        public ItemType Type => item.Type;
        public int LevelRequirement => item.LevelRequirement;
        public ItemFlags Flags => item.Flags;
        public ArenaRank PvpRankRequirement => item.PvpRankRequirement;
        public ArenaRank PetRankRequirement => item.PetRankRequirement;
        public School SchoolRequirement => item.SchoolRequirement;
        public School SchoolRestriction => item.SchoolRestriction;
        public bool IsRetired => item.IsRetired;
        public bool IsDebug => item.Flags.HasFlag(ItemFlags.FLAG_DevItem);
        public bool IsCrownsOnly => (item.Flags & ItemFlags.FLAG_CrownsOnly) == ItemFlags.FLAG_CrownsOnly;
        public bool IsPVPOnly => (item.Flags & ItemFlags.FLAG_PVPOnly) == ItemFlags.FLAG_PVPOnly;
        public static bool IsFavorite => false;
        public int MaxHealth => item.MaxHealth;
        public int MaxMana => item.MaxMana;
        public int MaxEnergy => item.MaxEnergy;
        public int SpeedBonus => item.SpeedBonus;
        public int PowerpipChance => item.PowerpipChance;
        public int ShadowpipRating => item.ShadowpipRating;
        public int StunResistChance => item.StunResistChance;
        public int FishingLuck => item.FishingLuck;
        public int ArchmasteryRating => item.ArchmasteryRating;
        public int IncomingHealing => item.IncomingHealing;
        public int OutgoingHealing => item.OutgoingHealing;
        public int PipsGiven => item.PipsGiven;
        public int PowerpipsGiven => item.PowerpipsGiven;
        public int TearJewelSlots => item.TearJewelSlots;
        public int CircleJewelSlots => item.CircleJewelSlots;
        public int SquareJewelSlots => item.SquareJewelSlots;
        public int TriangleJewelSlots => item.TriangleJewelSlots;
        public int PowerPinSlots => item.PowerPinSlots;
        public int ShieldPinSlots => item.ShieldPinSlots;
        public int SwordPinSlots => item.SwordPinSlots;
        public ItemSet SetBonus => item.SetBonus;
        public IReadOnlyDictionary<School, int> Accuracies => item.Accuracies;
        public IReadOnlyDictionary<School, int> Damages => item.Damages;
        public IReadOnlyDictionary<School, int> Resists => item.Resists;
        public IReadOnlyDictionary<School, int> Criticals => item.Criticals;
        public IReadOnlyDictionary<School, int> Blocks => item.Blocks;
        public IReadOnlyDictionary<School, int> Pierces => item.Pierces;
        public IReadOnlyDictionary<School, int> FlatDamages => item.FlatDamages;
        public IReadOnlyDictionary<School, int> FlatResists => item.FlatResists;
        public IReadOnlyDictionary<School, int> PipConversions => item.PipConversions;
        public IReadOnlyDictionary<string, int> ItemCards => item.ItemCards;
        public IReadOnlyCollection<School> AltSchoolMasteries => item.AltSchoolMasteries;

        public Uri SchoolIcon => item.SchoolRestriction == School.None ?
                                    item.SchoolRequirement.ToIconUri() :
                                    item.SchoolRestriction.ToSlashedIconUri();

        public string SchoolTooltip => item.SchoolRestriction == School.None ?
                                        item.SchoolRequirement.ToString() :
                                        $"Not {item.SchoolRestriction}";

        public Uri EquipmentTypeIcon => item.Type.ToIconUri();
    }
}
