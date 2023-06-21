//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;
//using System.Xml;
//using System.Xml.Serialization;
//using System.Linq;
//using Microsoft.Data.Sqlite;
//using System.Collections;
//using System.Xml.Linq;
//using GammaGear.Properties;
//using GammaGear.Source;
//using System.Windows.Input;
//using static GammaGear.Source.Item;
//using System.Data;
//using System.Windows.Media;
//using System.Diagnostics;

//namespace GammaGear.Source.Database
//{
//    public class XmlToDb
//    {

//        readonly List<Lang> _deserializedLangs = new List<Lang>();
//        public class Lang
//        {
//            public Lang()
//            {
//                Entries = new Dictionary<string, string>();
//            }
//            public Dictionary<string, string> Entries;
//            public string Name;
//        }
//        public string LoadDisplayName(string displayId, string localeFolder)
//        {
//            if (string.IsNullOrEmpty(displayId)) return "";

//            string fileName = displayId.Split('_')[0];
//            string id = string.Join('_', displayId.Split('_').Skip(1));
//            Lang lang = _deserializedLangs.Find(l => l.Name == fileName);
//            if (lang == null)
//            {
//                lang = new Lang
//                {
//                    Name = fileName
//                };
//                string fullPath = localeFolder + '\\' + fileName + ".lang";
//                using (var reader = new StreamReader(fullPath))
//                {
//                    Trace.WriteLine("Loaded " + localeFolder + '\\' + fileName + ".lang");
//                    // Trash the first line
//                    reader.ReadLine();

//                    // Entries go like this
//                    // ID \n Comment \n Content
//                    while (!reader.EndOfStream)
//                    {
//                        string newId = reader.ReadLine();
//                        reader.ReadLine();
//                        string newValue = reader.ReadLine();
//                        lang.Entries.Add(newId, newValue);
//                    }
//                    Trace.WriteLine("Unloaded " + localeFolder + '\\' + fileName + ".lang");
//                }
//                _deserializedLangs.Add(lang);
//            }
//            if (!lang.Entries.TryGetValue(id, out string value))
//            {
//                return "";
//            }
//            return lang.Entries[id];
//        }
//        public static PropertyClass[] DeserializeFile(string filePath)
//        {
//            // Get xml content into a string.
//            string xmlFileContent;
//            using (var fileReader = new StreamReader(filePath))
//            {
//                xmlFileContent = fileReader.ReadToEnd();
//            }

//            XmlDocument doc = new XmlDocument();
//            doc.LoadXml(xmlFileContent);
//            XmlNode rootNode = doc.DocumentElement;
//            // Get all nodes with the name "Class"
//            var nodes = rootNode.SelectNodes("//Class");

//            // Replace <Class Name="class foo"> with just the name of the class.
//            for (int i = nodes.Count - 1; i >= 0; i--)
//            {
//                XmlNode node = nodes[i];
//                // create new (renamed) Content node
//                XmlNode newNode = doc.CreateElement(node.Attributes[0].Value.Remove(0, 6));

//                // [if needed] copy existing Content children
//                newNode.InnerXml = node.InnerXml;

//                // replace existing Content node with newly renamed Content node
//                node.ParentNode.InsertBefore(newNode, node);
//                node.ParentNode.RemoveChild(node);
//            }

//            // Read the new xml and deserialize into a root object.
//            XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement);
//            XmlSerializer serializer = new XmlSerializer(typeof(ItemObject));
//            ItemObject root = (ItemObject)serializer.Deserialize(reader);

//            return root.m_propertyClasses.ToArray();
//        }
//        public string CreateDbFromItemList(List<PropertyClass> items, string dbLocation, string localeFolder)
//        {
//            bool dbSuccessful = true;
//            using (SqliteConnection db = new SqliteConnection($"Data Source=\"{dbLocation}\""))
//            {
//                db.Open();

//                var command = db.CreateCommand();

//                command.CommandText = SqliteQueries.CreateItemSetTable;
//                command.ExecuteNonQuery();

//                command.CommandText = SqliteQueries.CreateItemTable;
//                command.ExecuteNonQuery();

//                command.CommandText = SqliteQueries.CreateComplexStatTable;
//                command.ExecuteNonQuery();

//                command.CommandText = SqliteQueries.CreateSpellTable;
//                command.ExecuteNonQuery();

//                // Record every item and itemset parsed for reference recount later.
//                List<Item> itemsFinal = new List<Item>();
//                List<ItemSetBonus> itemSetsFinal = new List<ItemSetBonus>();
//                Dictionary<Guid, ItemSetBonus> replacementDict = new Dictionary<Guid, ItemSetBonus>(itemSetsFinal.Count);

//                // Parse all items and itemsets
//                foreach (PropertyClass item in items)
//                {
//                    if (item is WizItemTemplate wizItem)
//                    {
//                        Item newItem = new Item();
//                        // Get ID
//                        byte[] guidBytes = new byte[16];
//                        BitConverter.GetBytes(wizItem.m_templateID).CopyTo(guidBytes, 12);
//                        newItem.KiId = new Guid(guidBytes);

//                        // Set Bonus ID
//                        guidBytes = new byte[16];
//                        BitConverter.GetBytes(wizItem.m_itemSetBonusTemplateID).CopyTo(guidBytes, 12);
//                        newItem.KiSetBonusID = new Guid(guidBytes);

//                        // Get Display name
//                        newItem.Name = LoadDisplayName(wizItem.m_displayName, localeFolder);
//                        if (string.IsNullOrEmpty(newItem.Name))
//                        {
//                            newItem.Name = "Unnamed Item: " + wizItem.m_objectName;
//                            newItem.Flags |= ItemFlags.FLAG_DevItem;
//                        }
//                        if (newItem.Name.Contains("QA ") ||
//                            newItem.Name.ToLower() == "the one ring" ||
//                            newItem.Name.Contains("Test") ||
//                            newItem.Name.Contains("TEST"))
//                        {
//                            newItem.Flags |= ItemFlags.FLAG_DevItem;
//                        }

//                        // Get Description
//                        if (!string.IsNullOrEmpty(wizItem.m_description))
//                        {
//                            // TODO: XML Wiz Item Description to Database
//                            //newItem.Description = LoadDisplayName(wizItem.m_displayName, localeFolder);
//                        }


//                        // Tags
//                        foreach (string tag in wizItem.m_adjectiveList)
//                        {
//                            // Get flags
//                            bool found = false;
//                            foreach (Item.ItemType type in System.Enum.GetValues(typeof(Item.ItemType)))
//                            {
//                                if (tag == type.ToString())
//                                {
//                                    newItem.Type = type;
//                                    found = true;
//                                    break; // next tag
//                                }
//                            }
//                            if (found) continue;
//                            foreach (Item.ItemFlags flag in System.Enum.GetValues(typeof(Item.ItemFlags)))
//                            {
//                                if (tag == flag.ToString())
//                                {
//                                    newItem.Flags |= flag;
//                                    found = true;
//                                    break; // next tag
//                                }
//                            }
//                            if (found) continue;
//                            else if (tag == "Crafted")
//                            {
//                                newItem.Flags |= Item.ItemFlags.FLAG_Crafted;
//                            }
//                            else if (tag == "PVP")
//                            {
//                                newItem.Flags |= Item.ItemFlags.FLAG_PVPOnly;
//                            }
//                            //else if (tag == "NoPVP") // No PVP Items not introduced yet... May revisit in the future if these come out.
//                            //{
//                            //    newItem.Flags |= Item.ItemFlags.FLAG_PVPOnly;
//                            //}
//                        }

//                        // Equip Effects
//                        foreach (GameEffectInfo effectInfo in wizItem.m_equipEffects)
//                        {
//                            ParseEffectInfo(effectInfo, ref newItem);
//                        }

//                        // Behaviors
//                        foreach (BehaviorTemplate behavior in wizItem.m_behaviors)
//                        {
//                            if (behavior is JewelSocketBehaviorTemplate jewelBehavior)
//                            {
//                                foreach (JewelSocket socket in jewelBehavior.m_jewelSockets)
//                                {
//                                    switch (socket.m_socketType)
//                                    {
//                                        case JewelSocketType.SOCKETTYPE_SQUARE:
//                                            newItem.SquareJewelSlots += 1;
//                                            break;
//                                        case JewelSocketType.SOCKETTYPE_CIRCLE:
//                                            newItem.CircleJewelSlots += 1;
//                                            break;
//                                        case JewelSocketType.SOCKETTYPE_TRIANGLE:
//                                            newItem.TriangleJewelSlots += 1;
//                                            break;
//                                        case JewelSocketType.SOCKETTYPE_TEAR:
//                                            newItem.TearJewelSlots += 1;
//                                            break;
//                                        case JewelSocketType.SOCKETTYPE_PINSQUAREPIP:
//                                            newItem.PowerPinSlots += 1;
//                                            break;
//                                        case JewelSocketType.SOCKETTYPE_PINSQUARESHIELD:
//                                            newItem.ShieldPinSlots += 1;
//                                            break;
//                                        case JewelSocketType.SOCKETTYPE_PINSQUARESWORD:
//                                            newItem.SwordPinSlots += 1;
//                                            break;
//                                        case JewelSocketType.SOCKETTYPE_PET:
//                                        default:
//                                            Trace.WriteLine("Unhandled jewel type encountered: " + Enum.GetName(typeof(JewelSocketType), socket.m_socketType));
//                                            break;
//                                    }
//                                }
//                            }
//                        }

//                        // Equip requirements
//                        if (wizItem.m_equipRequirements.Count > 0)
//                        {
//                            foreach (Requirement requirement in wizItem.m_equipRequirements[0].m_requirements)
//                            {
//                                if (requirement is ReqMagicLevel reqMagicLevel)
//                                {
//                                    if (!string.IsNullOrEmpty(reqMagicLevel.m_magicSchool))
//                                    {
//                                        newItem.SchoolRequirement = Enum.Parse<Item.School>(reqMagicLevel.m_magicSchool);
//                                    }
//                                    newItem.LevelRequirement = (int)reqMagicLevel.m_numericValue;
//                                }
//                                else if (requirement is ReqSchoolOfFocus reqSchool)
//                                {
//                                    if (reqSchool.m_applyNOT == BooleanEnum.True)
//                                    {
//                                        newItem.SchoolRestriction = Enum.Parse<Item.School>(reqSchool.m_magicSchool);
//                                    }
//                                    else if (reqSchool.m_applyNOT == BooleanEnum.False)
//                                    {
//                                        newItem.SchoolRequirement = Enum.Parse<Item.School>(reqSchool.m_magicSchool);
//                                    }
//                                }
//                                else if (requirement is ReqHasBadge reqBadge)
//                                {
//                                    Trace.WriteLine("ReqHasBadge has not been implemented yet...");
//                                }
//                            }
//                        }

//                        //Trace.WriteLine("inserting Wiz Item : " + newItem.Name);
//                        itemsFinal.Add(newItem);
//                    }
//                    else if (item is ItemSetBonusTemplate itemSet)
//                    {
//                        ItemSetBonus newItemSet = new ItemSetBonus
//                        {
//                            // Display name
//                            SetName = LoadDisplayName(itemSet.m_displayName, localeFolder)
//                        };

//                        newItemSet.SetName = string.IsNullOrEmpty(newItemSet.SetName) ? "Unnamed Itemset: " + itemSet.m_objectName : newItemSet.SetName;

//                        // GUID
//                        byte[] guidBytes = new byte[16];
//                        BitConverter.GetBytes(itemSet.m_templateID).CopyTo(guidBytes, 12);
//                        newItemSet.KiId = new Guid(guidBytes);

//                        replacementDict.Add(newItemSet.KiId, newItemSet);

//                        // Bonuses
//                        for (int i = 0; i < itemSet.m_itemSetBonusDataList.Count; i++)
//                        {
//                            Item newItemBonus = new Item()
//                            {
//                                SetBonus = newItemSet,
//                                Type = Item.ItemType.ItemSetBonusData,
//                                SetBonusLevel = itemSet.m_itemSetBonusDataList[i].m_numItemsToEquip,
//                            };
//                            newItemBonus.Name = newItemSet.SetName + ": Tier " + newItemBonus.SetBonusLevel.ToString() + " Bonus";

//                            foreach (GameEffectInfo gfi in itemSet.m_itemSetBonusDataList[i].m_equipEffectsGranted)
//                            {
//                                ParseEffectInfo(gfi, ref newItemBonus);
//                            }

//                            newItemSet.Bonuses.Add(newItemBonus);
//                        }

//                        //Trace.WriteLine("inserting Wiz Itemset : " + newItemSet.SetName);
//                        //foreach (Item itemSetBonusData in newItemSet.Bonuses)
//                        //{
//                        //    itemsFinal.Add(itemSetBonusData);
//                        //}
//                        itemSetsFinal.Add(newItemSet);
//                    }
//                    else
//                    {
//                        Trace.WriteLine("Unknown item encountered when inserting in database: " + item);
//                    }
//                }

//                Trace.WriteLine("-------------------------------------------");
//                Trace.WriteLine("Renaming Duplicate Items...");
//                Trace.WriteLine("-------------------------------------------");

//                List<Item> nonUniqueNames = itemsFinal.Where(i => itemsFinal.Count(j => i.Name == j.Name) > 1).ToList();
//                foreach (Item item in nonUniqueNames)
//                {
//                    if (item.LevelRequirement == 1)
//                    {
//                        item.Name += " (Any Level)";
//                    }
//                    else
//                    {
//                        item.Name += " (Level " + item.LevelRequirement + "+)";
//                    }
//                }
//                // TODO: Fix this (single item dont get renamed) and possibly move to the program for more cohesive code.

//                Trace.WriteLine("-------------------------------------------");
//                Trace.WriteLine("Adding to Database...");
//                Trace.WriteLine("-------------------------------------------");

//                // Replace uint references with new GUIDs for better tracking.
//                var itemsWithSet = itemsFinal.Where(i => i.KiSetBonusID != Guid.Empty);
//                foreach (Item item in itemsWithSet)
//                {
//                    item.SetBonus = replacementDict[item.KiSetBonusID];
//                }

//                using (var transaction = db.BeginTransaction())
//                {
//                    // Items
//                    var commandInsertItem = db.CreateCommand();
//                    commandInsertItem.CommandText = SqliteQueries.InsertItem;

//                    var pItemId = commandInsertItem.CreateParameter();
//                    var pItemKiId = commandInsertItem.CreateParameter();
//                    var pItemType = commandInsertItem.CreateParameter();
//                    var pItemName = commandInsertItem.CreateParameter();
//                    var pItemLevelRequirement = commandInsertItem.CreateParameter();
//                    var pItemFlags = commandInsertItem.CreateParameter();
//                    var pItemPvpRankRequirement = commandInsertItem.CreateParameter();
//                    var pItemPetRankRequirement = commandInsertItem.CreateParameter();
//                    var pItemSchoolRequirement = commandInsertItem.CreateParameter();
//                    var pItemSchoolRestriction = commandInsertItem.CreateParameter();
//                    var pItemMaxHealth = commandInsertItem.CreateParameter();
//                    var pItemMaxMana = commandInsertItem.CreateParameter();
//                    var pItemMaxEnergy = commandInsertItem.CreateParameter();
//                    var pItemSpeedBonus = commandInsertItem.CreateParameter();
//                    var pItemPowerpipChance = commandInsertItem.CreateParameter();
//                    var pItemShadowpipRating = commandInsertItem.CreateParameter();
//                    var pItemStunResistChance = commandInsertItem.CreateParameter();
//                    var pItemFishingLuck = commandInsertItem.CreateParameter();
//                    var pItemArchmasteryRating = commandInsertItem.CreateParameter();
//                    var pItemIncomingHealing = commandInsertItem.CreateParameter();
//                    var pItemOutgoingHealing = commandInsertItem.CreateParameter();
//                    var pItemPipsGiven = commandInsertItem.CreateParameter();
//                    var pItemPowerpipsGiven = commandInsertItem.CreateParameter();
//                    var pItemAltSchoolMastery = commandInsertItem.CreateParameter();
//                    var pItemTearJewelSlots = commandInsertItem.CreateParameter();
//                    var pItemCircleJewelSlots = commandInsertItem.CreateParameter();
//                    var pItemSquareJewelSlots = commandInsertItem.CreateParameter();
//                    var pItemTriangleJewelSlots = commandInsertItem.CreateParameter();
//                    var pItemPowerPinSlots = commandInsertItem.CreateParameter();
//                    var pItemShieldPinSlots = commandInsertItem.CreateParameter();
//                    var pItemSwordPinSlots = commandInsertItem.CreateParameter();
//                    var pItemSetBonus = commandInsertItem.CreateParameter();
//                    var pItemKiSetBonusId = commandInsertItem.CreateParameter();
//                    var pItemIsSetBonusData = commandInsertItem.CreateParameter();
//                    var pItemSetBonusLevel = commandInsertItem.CreateParameter();
//                    pItemId.ParameterName = "$ID";
//                    pItemKiId.ParameterName = "$KI_ID";
//                    pItemType.ParameterName = "$Type";
//                    pItemName.ParameterName = "$Name";
//                    pItemLevelRequirement.ParameterName = "$LevelRequirement";
//                    pItemFlags.ParameterName = "$Flags";
//                    pItemPvpRankRequirement.ParameterName = "$PVPRankRequirement";
//                    pItemPetRankRequirement.ParameterName = "$PetRankRequirement";
//                    pItemSchoolRequirement.ParameterName = "$SchoolRequirement";
//                    pItemSchoolRestriction.ParameterName = "$SchoolRestriction";
//                    pItemMaxHealth.ParameterName = "$MaxHealth";
//                    pItemMaxMana.ParameterName = "$MaxMana";
//                    pItemMaxEnergy.ParameterName = "$MaxEnergy";
//                    pItemSpeedBonus.ParameterName = "$SpeedBonus";
//                    pItemPowerpipChance.ParameterName = "$PowerpipChance";
//                    pItemShadowpipRating.ParameterName = "$ShadowpipRating";
//                    pItemStunResistChance.ParameterName = "$StunResistChance";
//                    pItemFishingLuck.ParameterName = "$FishingLuck";
//                    pItemArchmasteryRating.ParameterName = "$ArchmasteryRating";
//                    pItemIncomingHealing.ParameterName = "$IncomingHealing";
//                    pItemOutgoingHealing.ParameterName = "$OutgoingHealing";
//                    pItemPipsGiven.ParameterName = "$PipsGiven";
//                    pItemPowerpipsGiven.ParameterName = "$PowerpipsGiven";
//                    pItemAltSchoolMastery.ParameterName = "$AltSchoolMastery";
//                    pItemTearJewelSlots.ParameterName = "$TearJewelSlots";
//                    pItemCircleJewelSlots.ParameterName = "$CircleJewelSlots";
//                    pItemSquareJewelSlots.ParameterName = "$SquareJewelSlots";
//                    pItemTriangleJewelSlots.ParameterName = "$TriangleJewelSlots";
//                    pItemPowerPinSlots.ParameterName = "$PowerPinSlots";
//                    pItemShieldPinSlots.ParameterName = "$ShieldPinSlots";
//                    pItemSwordPinSlots.ParameterName = "$SwordPinSlots";
//                    pItemSetBonus.ParameterName = "$SetBonus";
//                    pItemKiSetBonusId.ParameterName = "$KI_SetBonusID";
//                    pItemSetBonusLevel.ParameterName = "$SetBonusLevel";
//                    commandInsertItem.Parameters.Add(pItemId);
//                    commandInsertItem.Parameters.Add(pItemKiId);
//                    commandInsertItem.Parameters.Add(pItemType);
//                    commandInsertItem.Parameters.Add(pItemName);
//                    commandInsertItem.Parameters.Add(pItemLevelRequirement);
//                    commandInsertItem.Parameters.Add(pItemFlags);
//                    commandInsertItem.Parameters.Add(pItemPvpRankRequirement);
//                    commandInsertItem.Parameters.Add(pItemPetRankRequirement);
//                    commandInsertItem.Parameters.Add(pItemSchoolRequirement);
//                    commandInsertItem.Parameters.Add(pItemSchoolRestriction);
//                    commandInsertItem.Parameters.Add(pItemMaxHealth);
//                    commandInsertItem.Parameters.Add(pItemMaxMana);
//                    commandInsertItem.Parameters.Add(pItemMaxEnergy);
//                    commandInsertItem.Parameters.Add(pItemSpeedBonus);
//                    commandInsertItem.Parameters.Add(pItemPowerpipChance);
//                    commandInsertItem.Parameters.Add(pItemShadowpipRating);
//                    commandInsertItem.Parameters.Add(pItemStunResistChance);
//                    commandInsertItem.Parameters.Add(pItemFishingLuck);
//                    commandInsertItem.Parameters.Add(pItemArchmasteryRating);
//                    commandInsertItem.Parameters.Add(pItemIncomingHealing);
//                    commandInsertItem.Parameters.Add(pItemOutgoingHealing);
//                    commandInsertItem.Parameters.Add(pItemPipsGiven);
//                    commandInsertItem.Parameters.Add(pItemPowerpipsGiven);
//                    commandInsertItem.Parameters.Add(pItemAltSchoolMastery);
//                    commandInsertItem.Parameters.Add(pItemTearJewelSlots);
//                    commandInsertItem.Parameters.Add(pItemCircleJewelSlots);
//                    commandInsertItem.Parameters.Add(pItemSquareJewelSlots);
//                    commandInsertItem.Parameters.Add(pItemTriangleJewelSlots);
//                    commandInsertItem.Parameters.Add(pItemPowerPinSlots);
//                    commandInsertItem.Parameters.Add(pItemShieldPinSlots);
//                    commandInsertItem.Parameters.Add(pItemSwordPinSlots);
//                    commandInsertItem.Parameters.Add(pItemSetBonus);
//                    commandInsertItem.Parameters.Add(pItemKiSetBonusId);
//                    commandInsertItem.Parameters.Add(pItemSetBonusLevel);

//                    // Complex Stats
//                    var commandInsertComplexStat = db.CreateCommand();
//                    commandInsertComplexStat.CommandText = SqliteQueries.InsertComplexStat;

//                    var pComplexStatType = commandInsertComplexStat.CreateParameter();
//                    var pComplexStatSchool = commandInsertComplexStat.CreateParameter();
//                    var pComplexStatValue = commandInsertComplexStat.CreateParameter();
//                    var pComplexStatOldValue = commandInsertComplexStat.CreateParameter();
//                    var pComplexStatItem = commandInsertComplexStat.CreateParameter();
//                    commandInsertComplexStat.Parameters.Add(pComplexStatType);
//                    commandInsertComplexStat.Parameters.Add(pComplexStatSchool);
//                    commandInsertComplexStat.Parameters.Add(pComplexStatValue);
//                    commandInsertComplexStat.Parameters.Add(pComplexStatItem);
//                    pComplexStatType.ParameterName = "$Type";
//                    pComplexStatSchool.ParameterName = "$School";
//                    pComplexStatValue.ParameterName = "$Value";
//                    pComplexStatOldValue.ParameterName = "$OldValue";
//                    pComplexStatItem.ParameterName = "$Item";

//                    var commandSelectComplexStat = db.CreateCommand();
//                    commandSelectComplexStat.CommandText = "SELECT Value FROM ComplexStats WHERE Item=$Item AND Type=$Type AND Value=$Value AND School=$School";
//                    commandSelectComplexStat.Parameters.Add(pComplexStatType);
//                    commandSelectComplexStat.Parameters.Add(pComplexStatSchool);
//                    commandSelectComplexStat.Parameters.Add(pComplexStatValue);
//                    commandSelectComplexStat.Parameters.Add(pComplexStatItem);

//                    var commandUpdateComplexStat = db.CreateCommand();
//                    commandUpdateComplexStat.CommandText = "UPDATE ComplexStats SET Value=$Value WHERE Item=$Item AND Type=$Type AND School=$School AND Value=$OldValue";
//                    commandUpdateComplexStat.Parameters.Add(pComplexStatType);
//                    commandUpdateComplexStat.Parameters.Add(pComplexStatSchool);
//                    commandUpdateComplexStat.Parameters.Add(pComplexStatValue);
//                    commandUpdateComplexStat.Parameters.Add(pComplexStatOldValue);
//                    commandUpdateComplexStat.Parameters.Add(pComplexStatItem);

//                    // Spells
//                    var commandInsertSpell = db.CreateCommand();
//                    commandInsertSpell.CommandText = SqliteQueries.InsertSpell;

//                    var pSpellSpellName = commandInsertSpell.CreateParameter();
//                    var pSpellQuantity = commandInsertSpell.CreateParameter();
//                    var pSpellItem = commandInsertSpell.CreateParameter();
//                    commandInsertSpell.Parameters.Add(pSpellSpellName);
//                    commandInsertSpell.Parameters.Add(pSpellQuantity);
//                    commandInsertSpell.Parameters.Add(pSpellItem);
//                    pSpellSpellName.ParameterName = "$SpellName";
//                    pSpellQuantity.ParameterName = "$Quantity";
//                    pSpellItem.ParameterName = "$Item";

//                    var commandSelectSpell = db.CreateCommand();
//                    commandSelectSpell.CommandText = "SELECT Quantity FROM Spells WHERE Item=$Item AND SpellName=$SpellName AND Quantity=$Quantity";
//                    commandSelectSpell.Parameters.Add(pSpellSpellName);
//                    commandSelectSpell.Parameters.Add(pSpellQuantity);
//                    commandSelectSpell.Parameters.Add(pSpellItem);

//                    var commandUpdateSpell = db.CreateCommand();
//                    commandUpdateSpell.CommandText = "UPDATE Spells SET Quantity=$Quantity WHERE Item=$Item AND SpellName=$SpellName";
//                    commandUpdateSpell.Parameters.Add(pSpellSpellName);
//                    commandUpdateSpell.Parameters.Add(pSpellQuantity);
//                    commandUpdateSpell.Parameters.Add(pSpellItem);

//                    // Item Sets
//                    var commandInsertItemSet = db.CreateCommand();
//                    commandInsertItemSet.CommandText = SqliteQueries.InsertItemSet;

//                    var pItemSetId = commandInsertItemSet.CreateParameter();
//                    var pItemSetKiId = commandInsertItemSet.CreateParameter();
//                    var pItemSetName = commandInsertItemSet.CreateParameter();
//                    commandInsertItemSet.Parameters.Add(pItemSetId);
//                    commandInsertItemSet.Parameters.Add(pItemSetKiId);
//                    commandInsertItemSet.Parameters.Add(pItemSetName);
//                    pItemSetId.ParameterName = "$ID";
//                    pItemSetKiId.ParameterName = "$KI_ID";
//                    pItemSetName.ParameterName = "$Name";

//                    void InsertItem(Item item)
//                    {

//                        pItemId.Value = item.Id.ToString();
//                        pItemKiId.Value = item.KiId.ToString();
//                        pItemType.Value = (uint)item.Type;
//                        pItemName.Value = item.Name != null ? item.Name : DBNull.Value;
//                        pItemLevelRequirement.Value = item.LevelRequirement;
//                        pItemFlags.Value = (ushort)item.Flags;
//                        pItemPvpRankRequirement.Value = (byte)item.PvpRankRequirement;
//                        pItemPetRankRequirement.Value = (byte)item.PetRankRequirement;
//                        pItemSchoolRequirement.Value = (byte)item.SchoolRequirement;
//                        pItemSchoolRestriction.Value = (byte)item.SchoolRestriction;
//                        pItemMaxHealth.Value = item.MaxHealth;
//                        pItemMaxMana.Value = item.MaxMana;
//                        pItemMaxEnergy.Value = item.MaxEnergy;
//                        pItemSpeedBonus.Value = item.SpeedBonus;
//                        pItemPowerpipChance.Value = item.PowerpipChance;
//                        pItemShadowpipRating.Value = item.ShadowpipRating;
//                        pItemStunResistChance.Value = item.StunResistChance;
//                        pItemFishingLuck.Value = item.FishingLuck;
//                        pItemArchmasteryRating.Value = item.ArchmasteryRating;
//                        pItemIncomingHealing.Value = item.IncomingHealing;
//                        pItemOutgoingHealing.Value = item.OutgoingHealing;
//                        pItemPipsGiven.Value = item.PipsGiven;
//                        pItemPowerpipsGiven.Value = item.PowerpipsGiven;
//                        pItemAltSchoolMastery.Value = (byte)item.AltSchoolMastery;
//                        pItemTearJewelSlots.Value = item.TearJewelSlots;
//                        pItemCircleJewelSlots.Value = item.CircleJewelSlots;
//                        pItemSquareJewelSlots.Value = item.SquareJewelSlots;
//                        pItemTriangleJewelSlots.Value = item.TriangleJewelSlots;
//                        pItemPowerPinSlots.Value = item.PowerPinSlots;
//                        pItemShieldPinSlots.Value = item.ShieldPinSlots;
//                        pItemSwordPinSlots.Value = item.SwordPinSlots;
//                        pItemSetBonus.Value = item.SetBonus != null ? item.SetBonus.Id.ToString() : DBNull.Value;
//                        pItemKiSetBonusId.Value = item.KiSetBonusID.ToString();
//                        pItemSetBonusLevel.Value = item.SetBonusLevel;
//                        commandInsertItem.ExecuteNonQuery();

//                        void UpdateInsertComplex(Dictionary<Item.School, int> dict, Canonical type)
//                        {
//                            foreach (var entry in dict)
//                            {
//                                if (entry.Value < 1) continue;

//                                pComplexStatItem.Value = item.Id.ToString();
//                                pComplexStatSchool.Value = (byte)entry.Key;
//                                pComplexStatType.Value = (byte)type;
//                                pComplexStatValue.Value = entry.Value;
//                                long? quan = (long?)commandSelectComplexStat.ExecuteScalar();
//                                if (quan != null)
//                                {
//                                    pComplexStatOldValue.Value = pComplexStatValue.Value;
//                                    pComplexStatValue.Value = quan * 2;
//                                    commandUpdateComplexStat.ExecuteNonQuery();
//                                }
//                                else
//                                {
//                                    commandInsertComplexStat.ExecuteNonQuery();
//                                }
//                            }
//                        }

//                        UpdateInsertComplex(item.Accuracies, Canonical.Accuracy);
//                        UpdateInsertComplex(item.Damages, Canonical.Damage);
//                        UpdateInsertComplex(item.FlatDamages, Canonical.FlatDamage);
//                        UpdateInsertComplex(item.Resists, Canonical.ReduceDamage);
//                        UpdateInsertComplex(item.FlatResists, Canonical.FlatReduceDamage);
//                        UpdateInsertComplex(item.Pierces, Canonical.ArmorPiercing);
//                        UpdateInsertComplex(item.Criticals, Canonical.CriticalHit);
//                        UpdateInsertComplex(item.Blocks, Canonical.Block);
//                        UpdateInsertComplex(item.PipConversions, Canonical.PipConversion);

//                        foreach (var entry in item.ItemCards)
//                        {
//                            if (entry.Value < 1) continue;
//                            pSpellItem.Value = item.Id.ToString();
//                            pSpellSpellName.Value = entry.Key;
//                            pSpellQuantity.Value = entry.Value;
//                            long? quan = (long?)commandSelectSpell.ExecuteScalar();
//                            if (quan != null)
//                            {
//                                pSpellQuantity.Value = quan * 2;
//                                commandUpdateSpell.ExecuteNonQuery();
//                            }
//                            else
//                            {
//                                commandInsertSpell.ExecuteNonQuery();
//                            }
//                        }
//                    }


//                    foreach (var itemSet in itemSetsFinal)
//                    {
//                        pItemSetId.Value = itemSet.Id != Guid.Empty ? itemSet.Id.ToString() : DBNull.Value;
//                        pItemSetKiId.Value = itemSet.KiId != Guid.Empty ? itemSet.KiId.ToString() : DBNull.Value;
//                        pItemSetName.Value = itemSet.SetName;
//                        commandInsertItemSet.ExecuteNonQuery();

//                        foreach (var bonus in itemSet.Bonuses)
//                        {
//                            InsertItem(bonus);
//                        }
//                    }
//                    foreach (var item in itemsFinal)
//                    {
//                        InsertItem(item);
//                    }

//                    Trace.WriteLine("-------------------------------------------");
//                    Trace.WriteLine("Saving Database...");
//                    Trace.WriteLine("-------------------------------------------");

//                    transaction.Commit();
//                }

//                Trace.WriteLine("-------------------------------------------");
//                Trace.WriteLine("Done!");
//                Trace.WriteLine("-------------------------------------------");

//                db.Close();
//            }

//            return dbSuccessful ? dbLocation : "";
//        }
//        public string CreateDbFromList(string pathToList, string outputFileLocation)
//        {
//            // Get a list of all the files in the provided .txt file.
//            List<string> filesToDeserialize = new List<string>(1024);
//            using (var fileReader = new StreamReader(pathToList))
//            {
//                while (!fileReader.EndOfStream)
//                {
//                    filesToDeserialize.Add(fileReader.ReadLine());
//                }
//            }
//            string startingDirectory = filesToDeserialize[0];
//            filesToDeserialize.RemoveAt(0);

//            Trace.WriteLine("-------------------------------------------");
//            Trace.WriteLine("Reading in the XML files...");
//            Trace.WriteLine("-------------------------------------------");
//            // Deserialize all the files into PropertyClass objects
//            List<PropertyClass> newItems = new List<PropertyClass>(filesToDeserialize.Count);
//            foreach (string filePath in filesToDeserialize)
//            {
//                newItems.AddRange(DeserializeFile(startingDirectory + '\\' + filePath));
//            }
//            Trace.WriteLine("-------------------------------------------");
//            Trace.WriteLine("Constructing internal item lists...");
//            Trace.WriteLine("-------------------------------------------");
//            // Create DB and return the path to the DB.
//            return CreateDbFromItemList(newItems, outputFileLocation, Settings.Default.LocaleFolder);
//        }
//        public (List<Item>, List<ItemSetBonus>) CreateListFromDB(string dbLocation)
//        {
//            List<Item> items = new List<Item>();
//            Dictionary<Guid, Item> itemsDict = new Dictionary<Guid, Item>();
//            List<ItemSetBonus> itemSets = new List<ItemSetBonus>();
//            Dictionary<Guid, ItemSetBonus> itemSetsDict = new Dictionary<Guid, ItemSetBonus>();

//            // Open Database
//            using (SqliteConnection db = new SqliteConnection($"Data Source=\"{dbLocation}\""))
//            {
//                db.Open();
//                var command = db.CreateCommand();
//                command.CommandText = SqliteQueries.GetTableNames;
//                List<string> tableNames = new List<string>();

//                using (var reader = command.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        tableNames.Add(reader.GetString(0));
//                    }
//                }

//                // TODO: Do better validation that the tables are actually valid. Column types check and such
//                bool ItemsTableExists = tableNames.Contains("Items");
//                bool ComplexStatsTableExists = tableNames.Contains("ComplexStats");
//                bool SpellsTableExists = tableNames.Contains("Spells");
//                bool ItemSetsTableExists = tableNames.Contains("ItemSets");

//                if (ItemSetsTableExists)
//                {
//                    var getAllItemSetsCommand = db.CreateCommand();
//                    getAllItemSetsCommand.CommandText = "SELECT * FROM ItemSets";
//                    using (var reader = getAllItemSetsCommand.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            itemSets.Add(new ItemSetBonus()
//                            {
//                                Id = new Guid(reader.GetString(0)),
//                                KiId = new Guid(reader.GetString(1)),
//                                SetName = reader.GetString(2),
//                            });
//                        }
//                    }
//                    foreach (var itemSet in itemSets)
//                    {
//                        itemSetsDict.TryAdd(itemSet.Id, itemSet);
//                    }
//                }
//                if (ItemsTableExists)
//                {
//                    var getAllItemsCommand = db.CreateCommand();
//                    getAllItemsCommand.CommandText = "SELECT * FROM Items";
//                    using (var reader = getAllItemsCommand.ExecuteReader())
//                    {
//                        int ii = 1;
//                        while (reader.Read())
//                        {
//                            Item newItem = new Item()
//                            {
//                                Id = reader.GetGuid(0),
//                                KiId = reader.GetGuid(1),
//                                Type = (ItemType)reader.GetInt32(2),
//                                Name = reader.GetString(3),
//                                LevelRequirement = reader.GetInt32(4),
//                                Flags = (ItemFlags)reader.GetInt32(5),
//                                PvpRankRequirement = (ArenaRank)reader.GetInt32(6),
//                                PetRankRequirement = (ArenaRank)reader.GetInt32(7),
//                                SchoolRequirement = (School)reader.GetInt32(8),
//                                SchoolRestriction = (School)reader.GetInt32(9),
//                                MaxHealth = reader.GetInt32(10),
//                                MaxMana = reader.GetInt32(11),
//                                MaxEnergy = reader.GetInt32(12),
//                                SpeedBonus = reader.GetInt32(13),
//                                PowerpipChance = reader.GetInt32(14),
//                                ShadowpipRating = reader.GetInt32(15),
//                                StunResistChance = reader.GetInt32(16),
//                                FishingLuck = reader.GetInt32(17),
//                                ArchmasteryRating = reader.GetInt32(18),
//                                IncomingHealing = reader.GetInt32(19),
//                                OutgoingHealing = reader.GetInt32(20),
//                                PipsGiven = reader.GetInt32(21),
//                                PowerpipsGiven = reader.GetInt32(22),
//                                AltSchoolMastery = (School)reader.GetInt32(23),
//                                TearJewelSlots = reader.GetInt32(24),
//                                CircleJewelSlots = reader.GetInt32(25),
//                                SquareJewelSlots = reader.GetInt32(26),
//                                TriangleJewelSlots = reader.GetInt32(27),
//                                PowerPinSlots = reader.GetInt32(28),
//                                ShieldPinSlots = reader.GetInt32(29),
//                                SwordPinSlots = reader.GetInt32(30),
//                                SetBonus = !reader.IsDBNull(31) ? itemSetsDict[reader.GetGuid(31)] : null,
//                                KiSetBonusID = reader.GetGuid(32),
//                                SetBonusLevel = reader.GetInt32(33),
//                            };
//                            items.Add(newItem);
//                            ii++;
//                        }
//                    }
//                    foreach (var item in items)
//                    {
//                        itemsDict.TryAdd(item.Id, item);
//                    }
//                }
//                if (SpellsTableExists)
//                {
//                    var getAllSpellsCommand = db.CreateCommand();
//                    getAllSpellsCommand.CommandText = "SELECT * FROM Spells";
//                    using (var reader = getAllSpellsCommand.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            Guid itemID = reader.GetGuid(2);
//                            if (itemsDict.TryGetValue(itemID, out Item item))
//                            {
//                                item.ItemCards.AddOrIncrement(reader.GetString(0), reader.GetInt32(1));
//                            }
//                        }
//                    }
//                }
//                if (ComplexStatsTableExists)
//                {
//                    var getAllComplexStatsCommand = db.CreateCommand();
//                    getAllComplexStatsCommand.CommandText = "SELECT * FROM ComplexStats";
//                    using (var reader = getAllComplexStatsCommand.ExecuteReader())
//                    {
//                        while (reader.Read())
//                        {
//                            Guid itemID = reader.GetGuid(3);
//                            if (itemsDict.TryGetValue(itemID, out Item item))
//                            {
//                                item.GetDictionaryFromCanonical((Canonical)reader.GetInt32(0)).AddOrIncrement((School)reader.GetInt32(1), reader.GetInt32(2));
//                            }
//                        }
//                    }
//                }
//            }

//            foreach (var item in items.Where(i => i.SetBonus != null))
//            {
//                item.SetBonus.Bonuses.Add(item);
//            }

//            return (items, itemSets);
//        }

//        public static bool ParseEffectInfo(GameEffectInfo effectInfo, ref Item newItem)
//        {
//            if (effectInfo is StatisticEffectInfo stat)
//            {
//                bool found = false;
//                // Basic stats
//                for (Canonical i = Canonical.MetaStartUniversal + 1; i < Canonical.MetaEndUniversal; i++)
//                {
//                    if (found) break;
//                    if (stat.m_effectName == "Canonical" + Enum.GetName(typeof(Canonical), i))
//                    {
//                        found = true;
//                        switch (i)
//                        {
//                            case Canonical.MaxHealth:
//                                newItem.MaxHealth += stat.m_lookupIndex + 1;
//                                break;
//                            case Canonical.MaxMana:
//                                newItem.MaxMana += stat.m_lookupIndex + 1;
//                                break;
//                            case Canonical.MaxEnergy:
//                                newItem.MaxEnergy += stat.m_lookupIndex + 1;
//                                break;
//                            case Canonical.PowerPip:
//                                newItem.PowerpipChance += stat.m_lookupIndex - 99;
//                                break;
//                            case Canonical.IncHealing:
//                                newItem.IncomingHealing += stat.m_lookupIndex - 99;
//                                break;
//                            case Canonical.LifeHealing:
//                                newItem.OutgoingHealing += stat.m_lookupIndex - 99;
//                                break;
//                            case Canonical.ShadowPipRating:
//                                newItem.ShadowpipRating += stat.m_lookupIndex + 1;
//                                break;
//                            case Canonical.StunResistance:
//                                newItem.StunResistChance += stat.m_lookupIndex - 99;
//                                break;
//                            case Canonical.AllArchmastery:
//                                newItem.ArchmasteryRating += stat.m_lookupIndex + 1;
//                                break;
//                            case Canonical.AllFishingLuck:
//                                newItem.FishingLuck += stat.m_lookupIndex - 99;
//                                break;
//                            case Canonical.WispBonus:               // TODO: Implement WispBonus and MaxManaPercentReduce Canonicals
//                            case Canonical.MaxManaPercentReduce:
//                            default:
//                                Trace.WriteLine("Unhandled stat " + stat.m_effectName);
//                                break;
//                        }
//                    }
//                }
//                if (found) return true;

//                // Complex stats
//                for (Canonical i = Canonical.MetaStartSchoolSpecific + 1; i < Canonical.MetaEndSchoolSpecific; i++)
//                {
//                    for (Item.School j = Item.School.Any; j < Item.School.Sun; j++)
//                    {
//                        string sch = Enum.GetName(typeof(Item.School), j);
//                        sch = sch == "Any" || sch == "Universal" ? "All" : sch;
//                        if (stat.m_effectName == "Canonical" + sch + Enum.GetName(typeof(Canonical), i))
//                        {
//                            found = true;
//                            switch (i)
//                            {
//                                case Canonical.Accuracy:
//                                    newItem.Accuracies.AddOrIncrement(j, stat.m_lookupIndex - 99);
//                                    break;
//                                case Canonical.ArmorPiercing:
//                                    newItem.Pierces.AddOrIncrement(j, stat.m_lookupIndex - 99);
//                                    break;
//                                case Canonical.Block:
//                                    newItem.Blocks.AddOrIncrement(j, stat.m_lookupIndex + 1);
//                                    break;
//                                case Canonical.CriticalHit:
//                                    newItem.Criticals.AddOrIncrement(j, stat.m_lookupIndex + 1);
//                                    break;
//                                case Canonical.Damage:
//                                    newItem.Damages.AddOrIncrement(j, stat.m_lookupIndex - 99);
//                                    break;
//                                case Canonical.FlatDamage:
//                                    newItem.FlatDamages.AddOrIncrement(j, stat.m_lookupIndex + 1);
//                                    break;
//                                case Canonical.ReduceDamage:
//                                    newItem.Resists.AddOrIncrement(j, stat.m_lookupIndex - 99);
//                                    break;
//                                case Canonical.FlatReduceDamage:
//                                    newItem.FlatResists.AddOrIncrement(j, stat.m_lookupIndex + 1);
//                                    break;
//                                case Canonical.PipConversion:
//                                    newItem.PipConversions.AddOrIncrement(j, stat.m_lookupIndex + 1);
//                                    break;
//                                case Canonical.Mastery:
//                                    newItem.AltSchoolMastery = j;
//                                    break;
//                                default:
//                                    break;
//                            }
//                        }
//                        if (found) break;
//                    }
//                    if (found) break;
//                }
//                if (found) return true;
//            }
//            // Spells
//            else if (effectInfo is ProvideSpellEffectInfo spellInfo)
//            {
//                if (!newItem.ItemCards.TryAdd(spellInfo.m_spellName, spellInfo.m_numSpells))
//                {
//                    newItem.ItemCards[spellInfo.m_spellName] += spellInfo.m_numSpells;
//                }
//                return true;
//            }
//            // Powerpips
//            else if (effectInfo is StartingPipEffectInfo pipInfo)
//            {
//                newItem.PipsGiven += pipInfo.m_pipsGiven;
//                newItem.PowerpipsGiven += pipInfo.m_powerPipsGiven;
//                return true;
//            }
//            return false;
//        }
//    }
//}
