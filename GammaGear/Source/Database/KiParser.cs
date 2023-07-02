using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GammaGear.Source.Database
{
    public abstract class KiParser<T> : IKiParser
        where T: IKiLocaleBank
    {
        public KiParser(string localePath) { }
        public abstract PropertyClass ReadToPropertyClass(string path);
        public virtual IEnumerable<PropertyClass> ReadAllToPropertyClass(IEnumerable<string> paths)
        {
            List<PropertyClass> results = new List<PropertyClass>();
            foreach (string path in paths)
            {
                PropertyClass result = ReadToPropertyClass(path);
                if (result == null)
                    continue;
                results.Add(result);
            }
            return results;
        }
        public virtual KiObject ReadToKiObject(string path)
        {
            return ReadToKiObject(ReadToPropertyClass(path));
        }
        public virtual IEnumerable<KiObject> ReadAllToKiObject(IEnumerable<string> paths)
        {
            return ReadAllToKiObject(ReadAllToPropertyClass(paths));
        }
        public virtual KiObject ReadToKiObject(PropertyClass propertyClass)
        {
            KiObject result = null;

            if (propertyClass is WizItemTemplate wizItem)
            {
                Item newItem = new Item();
                // Get ID
                byte[] guidBytes = new byte[16];
                BitConverter.GetBytes(wizItem.m_templateID).CopyTo(guidBytes, 12);
                newItem.KiId = new Guid(guidBytes);

                // Set Bonus ID
                guidBytes = new byte[16];
                BitConverter.GetBytes(wizItem.m_itemSetBonusTemplateID).CopyTo(guidBytes, 12);
                newItem.KiSetBonusID = new Guid(guidBytes);

                // Get Display name
                newItem.Name = LoadDisplayName(wizItem.m_displayName);
                if (string.IsNullOrEmpty(newItem.Name))
                {
                    newItem.Name = "Unnamed Item: " + wizItem.m_objectName;
                    newItem.Flags |= Item.ItemFlags.FLAG_DevItem;
                }
                if (newItem.Name.Contains("QA ") ||
                    newItem.Name.ToLower() == "the one ring" ||
                    newItem.Name.Contains("Test") ||
                    newItem.Name.Contains("TEST"))
                {
                    newItem.Flags |= Item.ItemFlags.FLAG_DevItem;
                }

                // Get Description
                if (!string.IsNullOrEmpty(wizItem.m_description))
                {
                    // TODO: XML Wiz Item Description to Database
                    //newItem.Description = LoadDisplayName(wizItem.m_displayName, localeFolder);
                }

                // Tags
                foreach (string tag in wizItem.m_adjectiveList)
                {
                    // Get flags
                    bool found = false;
                    foreach (Item.ItemType type in System.Enum.GetValues(typeof(Item.ItemType)))
                    {
                        if (tag == type.ToString())
                        {
                            newItem.Type = type;
                            found = true;
                            break; // next tag
                        }
                    }
                    if (found) continue;
                    foreach (Item.ItemFlags flag in System.Enum.GetValues(typeof(Item.ItemFlags)))
                    {
                        if (tag == flag.ToString())
                        {
                            newItem.Flags |= flag;
                            found = true;
                            break; // next tag
                        }
                    }
                    if (found) continue;
                    else if (tag == "Crafted")
                    {
                        newItem.Flags |= Item.ItemFlags.FLAG_Crafted;
                    }
                    else if (tag == "PVP")
                    {
                        newItem.Flags |= Item.ItemFlags.FLAG_PVPOnly;
                    }
                    //else if (tag == "NoPVP") // No PVP Items not introduced yet... May revisit in the future if these come out.
                    //{
                    //    newItem.Flags |= Item.ItemFlags.FLAG_PVPOnly;
                    //}
                }

                // Equip Effects
                foreach (GameEffectInfo effectInfo in wizItem.m_equipEffects)
                {
                    ParseEffectInfo(effectInfo, ref newItem);
                }

                // Behaviors
                foreach (BehaviorTemplate behavior in wizItem.m_behaviors)
                {
                    if (behavior is JewelSocketBehaviorTemplate jewelBehavior)
                    {
                        foreach (JewelSocket socket in jewelBehavior.m_jewelSockets)
                        {
                            switch (socket.m_socketType)
                            {
                                case JewelSocketType.SOCKETTYPE_SQUARE:
                                    newItem.SquareJewelSlots += 1;
                                    break;
                                case JewelSocketType.SOCKETTYPE_CIRCLE:
                                    newItem.CircleJewelSlots += 1;
                                    break;
                                case JewelSocketType.SOCKETTYPE_TRIANGLE:
                                    newItem.TriangleJewelSlots += 1;
                                    break;
                                case JewelSocketType.SOCKETTYPE_TEAR:
                                    newItem.TearJewelSlots += 1;
                                    break;
                                case JewelSocketType.SOCKETTYPE_PINSQUAREPIP:
                                    newItem.PowerPinSlots += 1;
                                    break;
                                case JewelSocketType.SOCKETTYPE_PINSQUARESHIELD:
                                    newItem.ShieldPinSlots += 1;
                                    break;
                                case JewelSocketType.SOCKETTYPE_PINSQUARESWORD:
                                    newItem.SwordPinSlots += 1;
                                    break;
                                case JewelSocketType.SOCKETTYPE_PET:
                                default:
                                    Trace.WriteLine("Unhandled jewel type encountered: " + Enum.GetName(typeof(JewelSocketType), socket.m_socketType));
                                    break;
                            }
                        }
                    }
                }

                // Equip requirements
                foreach (Requirement requirement in wizItem.m_equipRequirements.m_requirements)
                {
                    if (requirement is ReqMagicLevel reqMagicLevel)
                    {
                        if (!string.IsNullOrEmpty(reqMagicLevel.m_magicSchool))
                        {
                            newItem.SchoolRequirement = Enum.Parse<Item.School>(reqMagicLevel.m_magicSchool);
                        }
                        newItem.LevelRequirement = (int)reqMagicLevel.m_numericValue;
                    }
                    else if (requirement is ReqSchoolOfFocus reqSchool)
                    {
                        if (reqSchool.m_applyNOT)
                        {
                            newItem.SchoolRestriction = Enum.Parse<Item.School>(reqSchool.m_magicSchool);
                        }
                        else
                        {
                            newItem.SchoolRequirement = Enum.Parse<Item.School>(reqSchool.m_magicSchool);
                        }
                    }
                    else if (requirement is ReqHasBadge)
                    {
                        Trace.WriteLine("ReqHasBadge has not been implemented yet...");
                    }
                }

                result = newItem;
            }
            else if (propertyClass is ItemSetBonusTemplate itemSet)
            {
                ItemSetBonus newItemSet = new ItemSetBonus
                {
                    // Display name
                    SetName = LoadDisplayName(itemSet.m_displayName)
                };

                newItemSet.SetName = string.IsNullOrEmpty(newItemSet.SetName) ? "Unnamed Itemset: " + itemSet.m_objectName : newItemSet.SetName;

                // GUID
                byte[] guidBytes = new byte[16];
                BitConverter.GetBytes(itemSet.m_templateID).CopyTo(guidBytes, 12);
                newItemSet.KiId = new Guid(guidBytes);

                // Bonuses
                for (int i = 0; i < itemSet.m_itemSetBonusDataList.Count; i++)
                {
                    Item newItemBonus = new Item()
                    {
                        SetBonus = newItemSet,
                        Type = Item.ItemType.ItemSetBonusData,
                        SetBonusLevel = itemSet.m_itemSetBonusDataList[i].m_numItemsToEquip,
                    };
                    newItemBonus.Name = newItemSet.SetName + ": Tier " + newItemBonus.SetBonusLevel.ToString() + " Bonus";

                    foreach (GameEffectInfo gfi in itemSet.m_itemSetBonusDataList[i].m_equipEffectsGranted)
                    {
                        ParseEffectInfo(gfi, ref newItemBonus);
                    }

                    newItemSet.Bonuses.Add(newItemBonus);
                }

                result = newItemSet;
            }
            else
            {
                Trace.WriteLine("Unknown item encountered when inserting in database: " + propertyClass);
            }

            return result;
        }
        public virtual IEnumerable<KiObject> ReadAllToKiObject(IEnumerable<PropertyClass> propertyClasses)
        {
            List<KiObject> results = new List<KiObject>();
            foreach (PropertyClass propertyClass in propertyClasses)
            {
                KiObject result = ReadToKiObject(propertyClass);
                if (result == null)
                    continue;
                results.Add(result);
            }
            return results;
        }
        protected virtual string LoadDisplayName(string displayId)
        {
            if (string.IsNullOrEmpty(displayId)) return "";

            string bankName = displayId.Split('_')[0];
            string id = string.Join('_', displayId.Split('_').Skip(1));
            IKiLocaleBank bank = _banks.GetValueOrDefault(bankName);
            if (bank == null)
            {
                return "";
            }

            if (!bank.Loaded)
            {
                bank.LoadEntries();
            }

            return bank.GetContent(id);
        }
        protected virtual bool ParseEffectInfo(GameEffectInfo effectInfo, ref Item newItem)
        {
            if (effectInfo is StatisticEffectInfo stat)
            {
                bool found = false;
                // Basic stats
                for (Canonical i = Canonical.MetaStartUniversal + 1; i < Canonical.MetaEndUniversal; i++)
                {
                    if (found) break;
                    if (stat.m_effectName == "Canonical" + Enum.GetName(typeof(Canonical), i))
                    {
                        found = true;
                        switch (i)
                        {
                            case Canonical.MaxHealth:
                                newItem.MaxHealth += stat.m_lookupIndex + 1;
                                break;
                            case Canonical.MaxMana:
                                newItem.MaxMana += stat.m_lookupIndex + 1;
                                break;
                            case Canonical.MaxEnergy:
                                newItem.MaxEnergy += stat.m_lookupIndex + 1;
                                break;
                            case Canonical.PowerPip:
                                newItem.PowerpipChance += stat.m_lookupIndex - 99;
                                break;
                            case Canonical.IncHealing:
                                newItem.IncomingHealing += stat.m_lookupIndex - 99;
                                break;
                            case Canonical.LifeHealing:
                                newItem.OutgoingHealing += stat.m_lookupIndex - 99;
                                break;
                            case Canonical.ShadowPipRating:
                                newItem.ShadowpipRating += stat.m_lookupIndex + 1;
                                break;
                            case Canonical.StunResistance:
                                newItem.StunResistChance += stat.m_lookupIndex - 99;
                                break;
                            case Canonical.AllArchmastery:
                                newItem.ArchmasteryRating += stat.m_lookupIndex + 1;
                                break;
                            case Canonical.AllFishingLuck:
                                newItem.FishingLuck += stat.m_lookupIndex - 99;
                                break;
                            case Canonical.WispBonus:               // TODO: Implement WispBonus and MaxManaPercentReduce Canonicals
                            case Canonical.MaxManaPercentReduce:
                            default:
                                Trace.WriteLine("Unhandled stat " + stat.m_effectName);
                                break;
                        }
                    }
                }
                if (found) return true;

                // Complex stats
                for (Canonical i = Canonical.MetaStartSchoolSpecific + 1; i < Canonical.MetaEndSchoolSpecific; i++)
                {
                    for (Item.School j = Item.School.Any; j < Item.School.Sun; j++)
                    {
                        string sch = Enum.GetName(typeof(Item.School), j);
                        sch = sch == "Any" || sch == "Universal" ? "All" : sch;
                        if (stat.m_effectName == "Canonical" + sch + Enum.GetName(typeof(Canonical), i))
                        {
                            found = true;
                            switch (i)
                            {
                                case Canonical.Accuracy:
                                    newItem.Accuracies.AddOrIncrement(j, stat.m_lookupIndex - 99);
                                    break;
                                case Canonical.ArmorPiercing:
                                    newItem.Pierces.AddOrIncrement(j, stat.m_lookupIndex - 99);
                                    break;
                                case Canonical.Block:
                                    newItem.Blocks.AddOrIncrement(j, stat.m_lookupIndex + 1);
                                    break;
                                case Canonical.CriticalHit:
                                    newItem.Criticals.AddOrIncrement(j, stat.m_lookupIndex + 1);
                                    break;
                                case Canonical.Damage:
                                    newItem.Damages.AddOrIncrement(j, stat.m_lookupIndex - 99);
                                    break;
                                case Canonical.FlatDamage:
                                    newItem.FlatDamages.AddOrIncrement(j, stat.m_lookupIndex + 1);
                                    break;
                                case Canonical.ReduceDamage:
                                    newItem.Resists.AddOrIncrement(j, stat.m_lookupIndex - 99);
                                    break;
                                case Canonical.FlatReduceDamage:
                                    newItem.FlatResists.AddOrIncrement(j, stat.m_lookupIndex + 1);
                                    break;
                                case Canonical.PipConversion:
                                    newItem.PipConversions.AddOrIncrement(j, stat.m_lookupIndex + 1);
                                    break;
                                case Canonical.Mastery:
                                    newItem.AltSchoolMastery = j;
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (found) break;
                    }
                    if (found) break;
                }
                if (found) return true;
            }
            // Spells
            else if (effectInfo is ProvideSpellEffectInfo spellInfo)
            {
                if (!newItem.ItemCards.TryAdd(spellInfo.m_spellName, spellInfo.m_numSpells))
                {
                    newItem.ItemCards[spellInfo.m_spellName] += spellInfo.m_numSpells;
                }
                return true;
            }
            // Powerpips
            else if (effectInfo is StartingPipEffectInfo pipInfo)
            {
                newItem.PipsGiven += pipInfo.m_pipsGiven;
                newItem.PowerpipsGiven += pipInfo.m_powerPipsGiven;
                return true;
            }
            return false;
        }

        protected Dictionary<string, T> _banks = new Dictionary<string, T>();
        protected DirectoryInfo _localeDirectory;
    }
}
