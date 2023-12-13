using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace GammaItems.Source.Database
{
    public enum RarityType
    {
        RT_COMMON = 0,
        RT_UNCOMMON = 1,
        RT_RARE = 2,
        RT_ULTRARARE = 3,
        RT_EPIC = 4
    }
    public enum OperatorType
    {
        ROP_AND = 0,
        ROP_OR = 1,
        Default = ROP_AND
    }

    public enum NumericOperatorType
    {
        OPERATOR_UNKNOWN = -1,
        OPERATOR_EQUALS = 0,
        OPERATOR_GREATER_THAN = 1,
        OPERATOR_LESS_THAN = 2,
        OPERATOR_GREATER_THAN_EQ = 3,
        OPERATOR_LESS_THAN_EQ = 4
    }
    public enum JewelSocketType
    {
        SOCKETTYPE_SQUARE = 0,
        SOCKETTYPE_CIRCLE = 1,
        SOCKETTYPE_TRIANGLE = 2,
        SOCKETTYPE_TEAR = 3,
        SOCKETTYPE_PET = 4,
        SOCKETTYPE_PINSQUAREPIP = 5,
        SOCKETTYPE_PINSQUARESHIELD = 6,
        SOCKETTYPE_PINSQUARESWORD = 7
    }
    public enum BooleanEnum
    {
        False = 0,
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
    public class PropertyClass { }
    public class JewelSocket : PropertyClass
    {
        public JewelSocketType m_socketType;

        // -> whether the socket starts off locked
        public bool m_bLockable;
    }
    public class BehaviorTemplate : PropertyClass
    {
        public string m_behaviorName;
    }
    public class PetItemBehaviorTemplate : BehaviorTemplate
    {
        public string m_eggName;

        public uint m_wowFactor;

        public List<string> m_favoriteSnackCategories = new List<string>();

        public bool MExclusivePet;
    }
    public class PetJewelBehaviorTemplate : BehaviorTemplate
    {
        public string m_exclusivePet;

        public byte m_minPetLevel;
    }
    public class JewelSocketBehaviorTemplate : BehaviorTemplate
    {
        public List<JewelSocket> m_jewelSockets = new List<JewelSocket>();

        public bool m_socketDeleted;
    }
    public class Requirement : PropertyClass
    {
        public bool m_applyNOT;

        public OperatorType m_operator;
    }
    public class ReqMagicLevel : Requirement
    {
        public float m_numericValue;

        public NumericOperatorType m_operatorType;

        public string m_magicSchool;
    }
    public class ReqHasBadge : Requirement
    {
        public string m_badgeName;
    }
    public class ReqSchoolOfFocus : Requirement
    {
        public string m_magicSchool;
    }
    public class RequirementList : PropertyClass
    {
        public bool m_applyNOT;

        public OperatorType m_operator;
        public List<Requirement> m_requirements = new List<Requirement>();
    }
    public class GameEffectInfo : PropertyClass
    {
        public string m_effectName;
    }
    public class StartingPipEffectInfo : GameEffectInfo
    {
        public int m_pipsGiven;
        public int m_powerPipsGiven;
    }
    public class TempStartingPipEffectInfo : StartingPipEffectInfo
    { }
    public class StatisticEffectInfo : GameEffectInfo
    {
        public int m_lookupIndex;
    }
    public class SpeedEffectInfo : GameEffectInfo
    {
        public int m_speedMultiplier;
    }
    public class ProvideSpellEffectInfo : GameEffectInfo
    {
        public string m_spellName;
        public int m_numSpells;
    }
    public class ItemSetBonusData : PropertyClass
    {
        public int m_numItemsToEquip;
        public string m_description;
        public List<RequirementList> m_equipEffectsGrantedRequirements = new List<RequirementList>();
        public List<GameEffectInfo> m_equipEffectsGranted = new List<GameEffectInfo>();
    }
    public class ItemSetBonusTemplate : PropertyClass
    {
        public List<BehaviorTemplate> m_behaviors = new List<BehaviorTemplate>();
        public string m_objectName;
        public uint m_templateID;
        public string m_displayName;
        public bool m_noStacking;
        public List<ItemSetBonusData> m_itemSetBonusDataList = new List<ItemSetBonusData>();
    }
    public class WizItemTemplate : PropertyClass
    {
        public List<BehaviorTemplate> m_behaviors = new List<BehaviorTemplate>();
        public string m_objectName;

        // Reference to this item for other items/itemsets or bundles to reference
        public uint m_templateID;

        // Tags
        // -> Item type -> ["Hat", "Robe", "Shoes", "Weapon", "Athame", "Amulet", "Ring", "Deck", "Pet", "Elixir", "Jewel"]
        // -> Jewel type -> CircleJewel, SquareJewel, TriangleJewel, TearJewel, PetJewel, PinSquareShield, PinSquareSword, PinSquarePower, PinSquarePip
        // -> FLAG -> FLAG_NoAuction | FLAG_NoBargain | FLAG_NoTrade | FLAG_Retired | FLAG_CrownsOnly | FLAG_NoSell | FLAG_NoDrop | FLAG_NoGift
        // -> NotForPetTome
        // -> SchoolMastery
        // -> Crafted
        public List<string> m_adjectiveList = new List<string>();

        // Reference to the locale data [Locale File]_[ID] -> WizardPets_00000025 -> "Armored Skeleton"
        public string m_displayName;

        // [LocaleFile]_[ID] -> purple text on lvl 125+ crafted wands
        public string m_description;

        public string m_sIcon;

        // Requirements to equip the item
        public RequirementList m_equipRequirements;

        // Benefits that the player receives when equipping this item
        public List<GameEffectInfo> m_equipEffects = new List<GameEffectInfo>();

        // m_itemSetBonusTemplateID -> int id, 0 if none
        public uint m_itemSetBonusTemplateID;

        // (for pets) school of pet -> Death, Balance, Storm, etc -> (for items) item's school (not the requirement though)
        public string m_school;

        // Game-given rarity
        public string m_rarity;
    }

    public class ItemObject
    {
        public List<PropertyClass> m_propertyClasses = new List<PropertyClass>();
    }
}
