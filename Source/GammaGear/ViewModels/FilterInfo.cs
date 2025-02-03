using GammaGear.Extensions;
using GammaItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaGear.ViewModels
{
    public enum FilterOption
    {
        Health,
        Mana,
        Energy,
        Damage,
        Resistance,
        Accuracy,
        PowerPipChance,
        CriticalRating,
        CriticalBlock,
        ArmorPiercing,
        PipConversion,
        ShadowPipRating,
        StunResistance,
        MovementSpeed,
        FishingLuck,
        ArchmasteryRating,
        FlatDamage,
        FlatResistance,
        Incoming,
        Outgoing,
        ExtraPips,
        ExtraPowerPips
    }
    public class FilterButtonInfo
    {
        public Uri ImageSource { get; set; }
        public string Tooltip { get; set; }
        public FilterOption FilterOption { get; set; }

        public FilterButtonInfo(string path, string tooltip, FilterOption filterOption)
        {
            ImageSource = new Uri(@"pack://application:,,,/GammaGear;component/" + path);
            Tooltip = tooltip;
            FilterOption = filterOption;
        }
    }
    public class FilterSchoolInfo
    {
        public Uri ImageSource { get; set; }
        public School Name { get; set; }

        public FilterSchoolInfo(School name)
        {
            ImageSource = name.ToIconUri();
            Name = name;
        }
    }
    public class FilterEquipmentInfo
    {
        public Uri ImageSource { get; set; }
        public ItemType Name { get; set; }

        public FilterEquipmentInfo(ItemType name)
        {
            ImageSource = name.ToIconUri();
            Name = name;
        }
    }
    public class FilterData
    {
        public Uri FilterOptionSource { get; set; }
        public Uri SchoolSource { get; set; }
        public FilterOption FilterOption { get; set; }
        public School School { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; }

        public FilterData(FilterOption filterOption, School school)
        {
            FilterOption = filterOption;
            School = school;
            SchoolSource = school.ToIconUri();
        }
    }
}
