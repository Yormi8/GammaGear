using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace GammaGear.Source.Database
{
    public enum RarityType
    {
        //[XmlEnum(Name = "enum RarityType::RT_COMMON")]
        RT_COMMON = 0,
        //[XmlEnum(Name = "enum RarityType::RT_UNCOMMON")]
        RT_UNCOMMON = 1,
        //[XmlEnum(Name = "enum RarityType::RT_RARE")]
        RT_RARE = 2,
        //[XmlEnum(Name = "enum RarityType::RT_ULTRARARE")]
        RT_ULTRARARE = 3,
        //[XmlEnum(Name = "enum RarityType::RT_EPIC")]
        RT_EPIC = 4
    }
    public enum OperatorType
    {
        //[XmlEnum(Name = "enum Requirement::Operator::ROP_AND")]
        ROP_AND = 0,
        //[XmlEnum(Name = "enum Requirement::Operator::ROP_OR")]
        ROP_OR = 1,
        Default = ROP_AND
    }

    public enum NumericOperatorType
    {
        //[XmlEnum(Name = "enum ReqNumeric::OPERATOR_TYPE::OPERATOR_UNKNOWN")]
        OPERATOR_UNKNOWN = -1,
        //[XmlEnum(Name = "enum ReqNumeric::OPERATOR_TYPE::OPERATOR_EQUALS")]
        OPERATOR_EQUALS = 0,
        //[XmlEnum(Name = "enum ReqNumeric::OPERATOR_TYPE::OPERATOR_GREATER_THAN")]
        OPERATOR_GREATER_THAN = 1,
        //[XmlEnum(Name = "enum ReqNumeric::OPERATOR_TYPE::OPERATOR_LESS_THAN")]
        OPERATOR_LESS_THAN = 2,
        //[XmlEnum(Name = "enum ReqNumeric::OPERATOR_TYPE::OPERATOR_GREATER_THAN_EQ")]
        OPERATOR_GREATER_THAN_EQ = 3,
        //[XmlEnum(Name = "enum ReqNumeric::OPERATOR_TYPE::OPERATOR_LESS_THAN_EQ")]
        OPERATOR_LESS_THAN_EQ = 4
    }
    public enum JewelSocketType
    {
        //[XmlEnum(Name = "enum JewelSocket::JewelSocketType::SOCKETTYPE_SQUARE")]
        SOCKETTYPE_SQUARE = 0,
        //[XmlEnum(Name = "enum JewelSocket::JewelSocketType::SOCKETTYPE_CIRCLE")]
        SOCKETTYPE_CIRCLE = 1,
        //[XmlEnum(Name = "enum JewelSocket::JewelSocketType::SOCKETTYPE_TRIANGLE")]
        SOCKETTYPE_TRIANGLE = 2,
        //[XmlEnum(Name = "enum JewelSocket::JewelSocketType::SOCKETTYPE_TEAR")]
        SOCKETTYPE_TEAR = 3,
        //[XmlEnum(Name = "enum JewelSocket::JewelSocketType::SOCKETTYPE_PET")]
        SOCKETTYPE_PET = 4,
        //[XmlEnum(Name = "enum JewelSocket::JewelSocketType::SOCKETTYPE_PINSQUAREPIP")]
        SOCKETTYPE_PINSQUAREPIP = 5,
        //[XmlEnum(Name = "enum JewelSocket::JewelSocketType::SOCKETTYPE_PINSQUARESHIELD")]
        SOCKETTYPE_PINSQUARESHIELD = 6,
        //[XmlEnum(Name = "enum JewelSocket::JewelSocketType::SOCKETTYPE_PINSQUARESWORD")]
        SOCKETTYPE_PINSQUARESWORD = 7
    }
    public enum BooleanEnum
    {
        //[XmlEnum(Name = "False")]
        False = 0,
        //[XmlEnum(Name = "True")]
        True = 1,
    }
    public enum Canonical
    {
        // School Specific
        MetaStartSchoolSpecific,

        Accuracy,           // Reduce 99
        ArmorPiercing,      // Reduce
        Block,              // Add 1
        CriticalHit,        // Add
        Damage,             // Reduce
        FlatDamage,         // Add
        ReduceDamage,       // Reduce
        FlatReduceDamage,   // Add
        PipConversion,      // Add
        Mastery,            // 0

        MetaEndSchoolSpecific,

        // Universal
        MetaStartUniversal,

        MaxHealth,              // Add
        MaxMana,                // Add
        MaxEnergy,              // Add
        PowerPip,               // Reduce
        IncHealing,             // Reduce
        LifeHealing,            // Reduce
        ShadowPipRating,        // Add
        StunResistance,         // Reduce
        AllArchmastery,         // Add
        AllFishingLuck,         // Reduce
        WispBonus,              // ? Reduce
        MaxManaPercentReduce,   // ? Reduce

        MetaEndUniversal,

    }
    [XmlType("PropertyClass")]
    public class PropertyClass { }
    [XmlType("JewelSocket")]
    public class JewelSocket : PropertyClass
    {
        [XmlElement("m_socketType")]
        public JewelSocketType m_socketType;

        // -> whether the socket starts off locked
        [XmlElement("m_bLockable")]
        public bool m_bLockable;
    }
    [XmlType("BehaviorTemplate")]
    public class BehaviorTemplate : PropertyClass
    {
        [XmlElement("m_behaviorName")]
        public string m_behaviorName;
    }
    [XmlType("PetItemBehaviorTemplate")]
    public class PetItemBehaviorTemplate : BehaviorTemplate
    {
        [XmlElement("m_eggName")]
        public string m_eggName;

        [XmlElement("m_wowFactor")]
        public uint m_wowFactor;

        [XmlElement("m_favoriteSnackCategories")]
        public List<string> m_favoriteSnackCategories = new List<string>();

        [XmlElement("m_exclusivePet")]
        public bool MExclusivePet;
    }
    [XmlType("PetJewelBehaviorTemplate")]
    public class PetJewelBehaviorTemplate : BehaviorTemplate
    {
        [XmlElement("m_petTalentName")]
        public string m_exclusivePet;

        [XmlElement("m_minPetLevel")]
        public byte m_minPetLevel;
    }
    [XmlType("JewelSocketBehaviorTemplate")]
    public class JewelSocketBehaviorTemplate : BehaviorTemplate
    {
        [XmlArrayItem("JewelSocket", typeof(JewelSocket))]
        public List<JewelSocket> m_jewelSockets = new List<JewelSocket>();

        [XmlElement("m_socketDeleted")]
        public bool m_socketDeleted;
    }
    [XmlType("Requirement")]
    public class Requirement : PropertyClass
    {
        [XmlElement("m_applyNOT")]
        public bool m_applyNOT;

        [XmlElement("m_operator")]
        public OperatorType m_operator;
    }
    [XmlType("ReqMagicLevel")]
    public class ReqMagicLevel : Requirement
    {
        [XmlElement("m_numericValue")]
        public float m_numericValue;

        [XmlElement("m_operatorType")]
        public NumericOperatorType m_operatorType;

        [XmlElement("m_magicSchool")]
        public string m_magicSchool;
    }
    [XmlType("ReqHasBadge")]
    public class ReqHasBadge : Requirement
    {
        [XmlElement("m_badgeName")]
        public string m_badgeName;
    }
    [XmlType("ReqSchoolOfFocus")]
    public class ReqSchoolOfFocus : Requirement
    {
        [XmlElement("m_magicSchool")]
        public string m_magicSchool;
    }
    [XmlType("RequirementList")]
    public class RequirementList : PropertyClass
    {
        [XmlElement("m_applyNOT")]
        public bool m_applyNOT;

        [XmlElement("m_operator")]
        public OperatorType m_operator;

        // TODO: This doesn't get populated when deserializing for some reason.
        [XmlArrayItem("ReqSchoolOfFocus", typeof(ReqSchoolOfFocus))]
        [XmlArrayItem("ReqMagicLevel", typeof(ReqMagicLevel))]
        [XmlArrayItem("ReqHasBadge", typeof(ReqHasBadge))]
        public List<Requirement> m_requirements = new List<Requirement>();
    }
    // ProvideCombatTriggerInfo?
    // SpeedEffectInfo?
    // ProvidePetPowerInfo?
    [XmlType("GameEffectInfo")]
    public class GameEffectInfo : PropertyClass
    {
        [XmlElement("m_effectName")]
        public string m_effectName;
    }
    [XmlType("StartingPipEffectInfo")]
    public class StartingPipEffectInfo : GameEffectInfo
    {
        [XmlElement("m_pipsGiven")]
        public int m_pipsGiven;

        [XmlElement("m_powerPipsGiven")]
        public int m_powerPipsGiven;
    }
    [XmlType("TempStartingPipEffectInfo")]
    public class TempStartingPipEffectInfo : StartingPipEffectInfo
    { }
    [XmlType("StatisticEffectInfo")]
    public class StatisticEffectInfo : GameEffectInfo
    {
        [XmlElement("m_lookupIndex")]
        public int m_lookupIndex;
    }
    [XmlType("SpeedEffectInfo")]
    public class SpeedEffectInfo : GameEffectInfo
    {
        [XmlElement("m_speedMultiplier")]
        public int m_speedMultiplier;
    }
    [XmlType("ProvideSpellEffectInfo")]
    public class ProvideSpellEffectInfo : GameEffectInfo
    {
        [XmlElement("m_spellName")]
        public string m_spellName;

        [XmlElement("m_numSpells")]
        public int m_numSpells;
    }
    [XmlType("ItemSetBonusData")]
    public class ItemSetBonusData : PropertyClass
    {
        [XmlElement("m_numItemsToEquip")]
        public int m_numItemsToEquip;

        [XmlElement("m_description")]
        public string m_description;

        [XmlArrayItem("RequirementList", typeof(RequirementList))]
        public List<RequirementList> m_equipEffectsGrantedRequirements = new List<RequirementList>();

        [XmlArrayItem("StatisticEffectInfo", typeof(StatisticEffectInfo))]
        [XmlArrayItem("ProvideSpellEffectInfo", typeof(ProvideSpellEffectInfo))]
        [XmlArrayItem("StartingPipEffectInfo", typeof(StartingPipEffectInfo))]
        [XmlArrayItem("TempStartingPipEffectInfo", typeof(TempStartingPipEffectInfo))]
        public List<GameEffectInfo> m_equipEffectsGranted = new List<GameEffectInfo>();
    }
    [XmlType("ItemSetBonusTemplate")]
    public class ItemSetBonusTemplate : PropertyClass
    {
        [XmlArrayItem("PetItemBehaviorTemplate", typeof(PetItemBehaviorTemplate))]
        [XmlArrayItem("PetJewelBehaviorTemplate", typeof(PetJewelBehaviorTemplate))]
        [XmlArrayItem("JewelSocketBehaviorTemplate", typeof(JewelSocketBehaviorTemplate))]
        public List<BehaviorTemplate> m_behaviors = new List<BehaviorTemplate>();

        [XmlElement("m_objectName")]
        public string m_objectName;

        [XmlElement("m_templateID")]
        public uint m_templateID;

        [XmlElement("m_displayName")]
        public string m_displayName;

        [XmlElement("m_noStacking")]
        public bool m_noStacking;

        [XmlArrayItem("ItemSetBonusData", typeof(ItemSetBonusData))]
        public List<ItemSetBonusData> m_itemSetBonusDataList = new List<ItemSetBonusData>();
    }
    [XmlType("WizItemTemplate")]
    public class WizItemTemplate : PropertyClass
    {
        [XmlArrayItem("PetItemBehaviorTemplate", typeof(PetItemBehaviorTemplate))]
        [XmlArrayItem("PetJewelBehaviorTemplate", typeof(PetJewelBehaviorTemplate))]
        [XmlArrayItem("JewelSocketBehaviorTemplate", typeof(JewelSocketBehaviorTemplate))]
        public List<BehaviorTemplate> m_behaviors = new List<BehaviorTemplate>();

        [XmlElement("m_objectName")]
        public string m_objectName;

        // Reference to this item for other items/itemsets or bundles to reference
        [XmlElement("m_templateID")]
        public uint m_templateID;

        // Tags
        // -> Item type -> ["Hat", "Robe", "Shoes", "Weapon", "Athame", "Amulet", "Ring", "Deck", "Pet", "Elixir", "Jewel"]
        // -> Jewel type -> CircleJewel, SquareJewel, TriangleJewel, TearJewel, PetJewel, PinSquareShield, PinSquareSword, PinSquarePower, PinSquarePip
        // -> FLAG -> FLAG_NoAuction | FLAG_NoBargain | FLAG_NoTrade | FLAG_Retired | FLAG_CrownsOnly | FLAG_NoSell | FLAG_NoDrop | FLAG_NoGift
        // -> NotForPetTome
        // -> SchoolMastery
        // -> Crafted
        [XmlElement("m_adjectiveList")]
        public List<string> m_adjectiveList = new List<string>();

        // Reference to the locale data [Locale File]_[ID] -> WizardPets_00000025 -> "Armored Skeleton"
        [XmlElement("m_displayName")]
        public string m_displayName;

        // [LocaleFile]_[ID] -> purple text on lvl 125+ crafted wands
        [XmlElement("m_description")]
        public string m_description;

        [XmlElement("m_sIcon")]
        public string m_sIcon;

        // Requirements to equip the item
        public RequirementList m_equipRequirements;

        // Benefits that the player receives when equipping this item
        [XmlArrayItem("StatisticEffectInfo", typeof(StatisticEffectInfo))]
        [XmlArrayItem("ProvideSpellEffectInfo", typeof(ProvideSpellEffectInfo))]
        [XmlArrayItem("StartingPipEffectInfo", typeof(StartingPipEffectInfo))]
        [XmlArrayItem("TempStartingPipEffectInfo", typeof(TempStartingPipEffectInfo))]
        public List<GameEffectInfo> m_equipEffects = new List<GameEffectInfo>();

        // m_itemSetBonusTemplateID -> int id, 0 if none
        [XmlElement("m_itemSetBonusTemplateID")]
        public uint m_itemSetBonusTemplateID;

        // (for pets) school of pet -> Death, Balance, Storm, etc -> (for items) item's school (not the requirement though)
        [XmlElement("m_school")]
        public string m_school;

        // Game-given rarity
        [XmlElement("m_rarity")]
        public string m_rarity;
    }

    [XmlRoot(ElementName = "ItemObject")]
    public class ItemObject
    {
        [XmlElement("WizItemTemplate", typeof(WizItemTemplate))]
        [XmlElement("ItemSetBonusTemplate", typeof(ItemSetBonusTemplate))]
        public List<PropertyClass> m_propertyClasses = new List<PropertyClass>();
    }
}
