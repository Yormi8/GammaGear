using Microsoft.Data.Sqlite;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GammaGear.Source.Item;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using System.IO;

namespace GammaGear.Source.Database
{
    public class KiSqliteWriter : KiWriter
    {
        public override bool Write(string path, IEnumerable<KiObject> values)
        {
            using (SqliteConnection db = new SqliteConnection($"Data Source=\"{path}\""))
            {
                db.Open();

                var command = db.CreateCommand();

                command.CommandText = SqliteQueries.CreateItemSetTable;
                command.ExecuteNonQuery();

                command.CommandText = SqliteQueries.CreateItemTable;
                command.ExecuteNonQuery();

                command.CommandText = SqliteQueries.CreateComplexStatTable;
                command.ExecuteNonQuery();

                command.CommandText = SqliteQueries.CreateSpellTable;
                command.ExecuteNonQuery();

                // Record every item and itemset parsed for reference recount later.
                List<Item> itemsFinal = new List<Item>();
                List<ItemSetBonus> itemSetsFinal = new List<ItemSetBonus>();
                Dictionary<Guid, ItemSetBonus> replacementDict = new Dictionary<Guid, ItemSetBonus>(itemSetsFinal.Count);

                List<Item> nonUniqueNames = itemsFinal.Where(i => itemsFinal.Count(j => i.Name == j.Name) > 1).ToList();
                foreach (Item item in nonUniqueNames)
                {
                    if (item.LevelRequirement == 1)
                    {
                        item.Name += " (Any Level)";
                    }
                    else
                    {
                        item.Name += " (Level " + item.LevelRequirement + "+)";
                    }
                }
                // TODO: Fix this (single item dont get renamed) and possibly move to the program for more cohesive code.

                // Replace uint references with new GUIDs for better tracking.
                var itemsWithSet = itemsFinal.Where(i => i.KiSetBonusID != Guid.Empty);
                foreach (Item item in itemsWithSet)
                {
                    item.SetBonus = replacementDict[item.KiSetBonusID];
                }

                using (var transaction = db.BeginTransaction())
                {
                    // Items
                    var commandInsertItem = db.CreateCommand();
                    commandInsertItem.CommandText = SqliteQueries.InsertItem;

                    var pItemId = commandInsertItem.CreateParameter();
                    var pItemKiId = commandInsertItem.CreateParameter();
                    var pItemType = commandInsertItem.CreateParameter();
                    var pItemName = commandInsertItem.CreateParameter();
                    var pItemLevelRequirement = commandInsertItem.CreateParameter();
                    var pItemFlags = commandInsertItem.CreateParameter();
                    var pItemPvpRankRequirement = commandInsertItem.CreateParameter();
                    var pItemPetRankRequirement = commandInsertItem.CreateParameter();
                    var pItemSchoolRequirement = commandInsertItem.CreateParameter();
                    var pItemSchoolRestriction = commandInsertItem.CreateParameter();
                    var pItemMaxHealth = commandInsertItem.CreateParameter();
                    var pItemMaxMana = commandInsertItem.CreateParameter();
                    var pItemMaxEnergy = commandInsertItem.CreateParameter();
                    var pItemSpeedBonus = commandInsertItem.CreateParameter();
                    var pItemPowerpipChance = commandInsertItem.CreateParameter();
                    var pItemShadowpipRating = commandInsertItem.CreateParameter();
                    var pItemStunResistChance = commandInsertItem.CreateParameter();
                    var pItemFishingLuck = commandInsertItem.CreateParameter();
                    var pItemArchmasteryRating = commandInsertItem.CreateParameter();
                    var pItemIncomingHealing = commandInsertItem.CreateParameter();
                    var pItemOutgoingHealing = commandInsertItem.CreateParameter();
                    var pItemPipsGiven = commandInsertItem.CreateParameter();
                    var pItemPowerpipsGiven = commandInsertItem.CreateParameter();
                    var pItemAltSchoolMastery = commandInsertItem.CreateParameter();
                    var pItemTearJewelSlots = commandInsertItem.CreateParameter();
                    var pItemCircleJewelSlots = commandInsertItem.CreateParameter();
                    var pItemSquareJewelSlots = commandInsertItem.CreateParameter();
                    var pItemTriangleJewelSlots = commandInsertItem.CreateParameter();
                    var pItemPowerPinSlots = commandInsertItem.CreateParameter();
                    var pItemShieldPinSlots = commandInsertItem.CreateParameter();
                    var pItemSwordPinSlots = commandInsertItem.CreateParameter();
                    var pItemSetBonus = commandInsertItem.CreateParameter();
                    var pItemKiSetBonusId = commandInsertItem.CreateParameter();
                    var pItemIsSetBonusData = commandInsertItem.CreateParameter();
                    var pItemSetBonusLevel = commandInsertItem.CreateParameter();
                    pItemId.ParameterName = "$ID";
                    pItemKiId.ParameterName = "$KI_ID";
                    pItemType.ParameterName = "$Type";
                    pItemName.ParameterName = "$Name";
                    pItemLevelRequirement.ParameterName = "$LevelRequirement";
                    pItemFlags.ParameterName = "$Flags";
                    pItemPvpRankRequirement.ParameterName = "$PVPRankRequirement";
                    pItemPetRankRequirement.ParameterName = "$PetRankRequirement";
                    pItemSchoolRequirement.ParameterName = "$SchoolRequirement";
                    pItemSchoolRestriction.ParameterName = "$SchoolRestriction";
                    pItemMaxHealth.ParameterName = "$MaxHealth";
                    pItemMaxMana.ParameterName = "$MaxMana";
                    pItemMaxEnergy.ParameterName = "$MaxEnergy";
                    pItemSpeedBonus.ParameterName = "$SpeedBonus";
                    pItemPowerpipChance.ParameterName = "$PowerpipChance";
                    pItemShadowpipRating.ParameterName = "$ShadowpipRating";
                    pItemStunResistChance.ParameterName = "$StunResistChance";
                    pItemFishingLuck.ParameterName = "$FishingLuck";
                    pItemArchmasteryRating.ParameterName = "$ArchmasteryRating";
                    pItemIncomingHealing.ParameterName = "$IncomingHealing";
                    pItemOutgoingHealing.ParameterName = "$OutgoingHealing";
                    pItemPipsGiven.ParameterName = "$PipsGiven";
                    pItemPowerpipsGiven.ParameterName = "$PowerpipsGiven";
                    pItemAltSchoolMastery.ParameterName = "$AltSchoolMastery";
                    pItemTearJewelSlots.ParameterName = "$TearJewelSlots";
                    pItemCircleJewelSlots.ParameterName = "$CircleJewelSlots";
                    pItemSquareJewelSlots.ParameterName = "$SquareJewelSlots";
                    pItemTriangleJewelSlots.ParameterName = "$TriangleJewelSlots";
                    pItemPowerPinSlots.ParameterName = "$PowerPinSlots";
                    pItemShieldPinSlots.ParameterName = "$ShieldPinSlots";
                    pItemSwordPinSlots.ParameterName = "$SwordPinSlots";
                    pItemSetBonus.ParameterName = "$SetBonus";
                    pItemKiSetBonusId.ParameterName = "$KI_SetBonusID";
                    pItemSetBonusLevel.ParameterName = "$SetBonusLevel";
                    commandInsertItem.Parameters.Add(pItemId);
                    commandInsertItem.Parameters.Add(pItemKiId);
                    commandInsertItem.Parameters.Add(pItemType);
                    commandInsertItem.Parameters.Add(pItemName);
                    commandInsertItem.Parameters.Add(pItemLevelRequirement);
                    commandInsertItem.Parameters.Add(pItemFlags);
                    commandInsertItem.Parameters.Add(pItemPvpRankRequirement);
                    commandInsertItem.Parameters.Add(pItemPetRankRequirement);
                    commandInsertItem.Parameters.Add(pItemSchoolRequirement);
                    commandInsertItem.Parameters.Add(pItemSchoolRestriction);
                    commandInsertItem.Parameters.Add(pItemMaxHealth);
                    commandInsertItem.Parameters.Add(pItemMaxMana);
                    commandInsertItem.Parameters.Add(pItemMaxEnergy);
                    commandInsertItem.Parameters.Add(pItemSpeedBonus);
                    commandInsertItem.Parameters.Add(pItemPowerpipChance);
                    commandInsertItem.Parameters.Add(pItemShadowpipRating);
                    commandInsertItem.Parameters.Add(pItemStunResistChance);
                    commandInsertItem.Parameters.Add(pItemFishingLuck);
                    commandInsertItem.Parameters.Add(pItemArchmasteryRating);
                    commandInsertItem.Parameters.Add(pItemIncomingHealing);
                    commandInsertItem.Parameters.Add(pItemOutgoingHealing);
                    commandInsertItem.Parameters.Add(pItemPipsGiven);
                    commandInsertItem.Parameters.Add(pItemPowerpipsGiven);
                    commandInsertItem.Parameters.Add(pItemAltSchoolMastery);
                    commandInsertItem.Parameters.Add(pItemTearJewelSlots);
                    commandInsertItem.Parameters.Add(pItemCircleJewelSlots);
                    commandInsertItem.Parameters.Add(pItemSquareJewelSlots);
                    commandInsertItem.Parameters.Add(pItemTriangleJewelSlots);
                    commandInsertItem.Parameters.Add(pItemPowerPinSlots);
                    commandInsertItem.Parameters.Add(pItemShieldPinSlots);
                    commandInsertItem.Parameters.Add(pItemSwordPinSlots);
                    commandInsertItem.Parameters.Add(pItemSetBonus);
                    commandInsertItem.Parameters.Add(pItemKiSetBonusId);
                    commandInsertItem.Parameters.Add(pItemSetBonusLevel);

                    // Complex Stats
                    var commandInsertComplexStat = db.CreateCommand();
                    commandInsertComplexStat.CommandText = SqliteQueries.InsertComplexStat;

                    var pComplexStatType = commandInsertComplexStat.CreateParameter();
                    var pComplexStatSchool = commandInsertComplexStat.CreateParameter();
                    var pComplexStatValue = commandInsertComplexStat.CreateParameter();
                    var pComplexStatOldValue = commandInsertComplexStat.CreateParameter();
                    var pComplexStatItem = commandInsertComplexStat.CreateParameter();
                    commandInsertComplexStat.Parameters.Add(pComplexStatType);
                    commandInsertComplexStat.Parameters.Add(pComplexStatSchool);
                    commandInsertComplexStat.Parameters.Add(pComplexStatValue);
                    commandInsertComplexStat.Parameters.Add(pComplexStatItem);
                    pComplexStatType.ParameterName = "$Type";
                    pComplexStatSchool.ParameterName = "$School";
                    pComplexStatValue.ParameterName = "$Value";
                    pComplexStatOldValue.ParameterName = "$OldValue";
                    pComplexStatItem.ParameterName = "$Item";

                    var commandSelectComplexStat = db.CreateCommand();
                    commandSelectComplexStat.CommandText = "SELECT Value FROM ComplexStats WHERE Item=$Item AND Type=$Type AND Value=$Value AND School=$School";
                    commandSelectComplexStat.Parameters.Add(pComplexStatType);
                    commandSelectComplexStat.Parameters.Add(pComplexStatSchool);
                    commandSelectComplexStat.Parameters.Add(pComplexStatValue);
                    commandSelectComplexStat.Parameters.Add(pComplexStatItem);

                    var commandUpdateComplexStat = db.CreateCommand();
                    commandUpdateComplexStat.CommandText = "UPDATE ComplexStats SET Value=$Value WHERE Item=$Item AND Type=$Type AND School=$School AND Value=$OldValue";
                    commandUpdateComplexStat.Parameters.Add(pComplexStatType);
                    commandUpdateComplexStat.Parameters.Add(pComplexStatSchool);
                    commandUpdateComplexStat.Parameters.Add(pComplexStatValue);
                    commandUpdateComplexStat.Parameters.Add(pComplexStatOldValue);
                    commandUpdateComplexStat.Parameters.Add(pComplexStatItem);

                    // Spells
                    var commandInsertSpell = db.CreateCommand();
                    commandInsertSpell.CommandText = SqliteQueries.InsertSpell;

                    var pSpellSpellName = commandInsertSpell.CreateParameter();
                    var pSpellQuantity = commandInsertSpell.CreateParameter();
                    var pSpellItem = commandInsertSpell.CreateParameter();
                    commandInsertSpell.Parameters.Add(pSpellSpellName);
                    commandInsertSpell.Parameters.Add(pSpellQuantity);
                    commandInsertSpell.Parameters.Add(pSpellItem);
                    pSpellSpellName.ParameterName = "$SpellName";
                    pSpellQuantity.ParameterName = "$Quantity";
                    pSpellItem.ParameterName = "$Item";

                    var commandSelectSpell = db.CreateCommand();
                    commandSelectSpell.CommandText = "SELECT Quantity FROM Spells WHERE Item=$Item AND SpellName=$SpellName AND Quantity=$Quantity";
                    commandSelectSpell.Parameters.Add(pSpellSpellName);
                    commandSelectSpell.Parameters.Add(pSpellQuantity);
                    commandSelectSpell.Parameters.Add(pSpellItem);

                    var commandUpdateSpell = db.CreateCommand();
                    commandUpdateSpell.CommandText = "UPDATE Spells SET Quantity=$Quantity WHERE Item=$Item AND SpellName=$SpellName";
                    commandUpdateSpell.Parameters.Add(pSpellSpellName);
                    commandUpdateSpell.Parameters.Add(pSpellQuantity);
                    commandUpdateSpell.Parameters.Add(pSpellItem);

                    // Item Sets
                    var commandInsertItemSet = db.CreateCommand();
                    commandInsertItemSet.CommandText = SqliteQueries.InsertItemSet;

                    var pItemSetId = commandInsertItemSet.CreateParameter();
                    var pItemSetKiId = commandInsertItemSet.CreateParameter();
                    var pItemSetName = commandInsertItemSet.CreateParameter();
                    commandInsertItemSet.Parameters.Add(pItemSetId);
                    commandInsertItemSet.Parameters.Add(pItemSetKiId);
                    commandInsertItemSet.Parameters.Add(pItemSetName);
                    pItemSetId.ParameterName = "$ID";
                    pItemSetKiId.ParameterName = "$KI_ID";
                    pItemSetName.ParameterName = "$Name";

                    void InsertItem(Item item)
                    {

                        pItemId.Value = item.Id.ToString();
                        pItemKiId.Value = item.KiId.ToString();
                        pItemType.Value = (uint)item.Type;
                        pItemName.Value = item.Name != null ? item.Name : DBNull.Value;
                        pItemLevelRequirement.Value = item.LevelRequirement;
                        pItemFlags.Value = (ushort)item.Flags;
                        pItemPvpRankRequirement.Value = (byte)item.PvpRankRequirement;
                        pItemPetRankRequirement.Value = (byte)item.PetRankRequirement;
                        pItemSchoolRequirement.Value = (byte)item.SchoolRequirement;
                        pItemSchoolRestriction.Value = (byte)item.SchoolRestriction;
                        pItemMaxHealth.Value = item.MaxHealth;
                        pItemMaxMana.Value = item.MaxMana;
                        pItemMaxEnergy.Value = item.MaxEnergy;
                        pItemSpeedBonus.Value = item.SpeedBonus;
                        pItemPowerpipChance.Value = item.PowerpipChance;
                        pItemShadowpipRating.Value = item.ShadowpipRating;
                        pItemStunResistChance.Value = item.StunResistChance;
                        pItemFishingLuck.Value = item.FishingLuck;
                        pItemArchmasteryRating.Value = item.ArchmasteryRating;
                        pItemIncomingHealing.Value = item.IncomingHealing;
                        pItemOutgoingHealing.Value = item.OutgoingHealing;
                        pItemPipsGiven.Value = item.PipsGiven;
                        pItemPowerpipsGiven.Value = item.PowerpipsGiven;
                        pItemAltSchoolMastery.Value = (byte)item.AltSchoolMastery;
                        pItemTearJewelSlots.Value = item.TearJewelSlots;
                        pItemCircleJewelSlots.Value = item.CircleJewelSlots;
                        pItemSquareJewelSlots.Value = item.SquareJewelSlots;
                        pItemTriangleJewelSlots.Value = item.TriangleJewelSlots;
                        pItemPowerPinSlots.Value = item.PowerPinSlots;
                        pItemShieldPinSlots.Value = item.ShieldPinSlots;
                        pItemSwordPinSlots.Value = item.SwordPinSlots;
                        pItemSetBonus.Value = item.SetBonus != null ? item.SetBonus.Id.ToString() : DBNull.Value;
                        pItemKiSetBonusId.Value = item.KiSetBonusID.ToString();
                        pItemSetBonusLevel.Value = item.SetBonusLevel;
                        commandInsertItem.ExecuteNonQuery();

                        void UpdateInsertComplex(Dictionary<Item.School, int> dict, Canonical type)
                        {
                            foreach (var entry in dict)
                            {
                                if (entry.Value < 1) continue;

                                pComplexStatItem.Value = item.Id.ToString();
                                pComplexStatSchool.Value = (byte)entry.Key;
                                pComplexStatType.Value = (byte)type;
                                pComplexStatValue.Value = entry.Value;
                                long? quan = (long?)commandSelectComplexStat.ExecuteScalar();
                                if (quan != null)
                                {
                                    pComplexStatOldValue.Value = pComplexStatValue.Value;
                                    pComplexStatValue.Value = quan * 2;
                                    commandUpdateComplexStat.ExecuteNonQuery();
                                }
                                else
                                {
                                    commandInsertComplexStat.ExecuteNonQuery();
                                }
                            }
                        }

                        UpdateInsertComplex(item.Accuracies, Canonical.Accuracy);
                        UpdateInsertComplex(item.Damages, Canonical.Damage);
                        UpdateInsertComplex(item.FlatDamages, Canonical.FlatDamage);
                        UpdateInsertComplex(item.Resists, Canonical.ReduceDamage);
                        UpdateInsertComplex(item.FlatResists, Canonical.FlatReduceDamage);
                        UpdateInsertComplex(item.Pierces, Canonical.ArmorPiercing);
                        UpdateInsertComplex(item.Criticals, Canonical.CriticalHit);
                        UpdateInsertComplex(item.Blocks, Canonical.Block);
                        UpdateInsertComplex(item.PipConversions, Canonical.PipConversion);

                        foreach (var entry in item.ItemCards)
                        {
                            if (entry.Value < 1) continue;
                            pSpellItem.Value = item.Id.ToString();
                            pSpellSpellName.Value = entry.Key;
                            pSpellQuantity.Value = entry.Value;
                            long? quan = (long?)commandSelectSpell.ExecuteScalar();
                            if (quan != null)
                            {
                                pSpellQuantity.Value = quan * 2;
                                commandUpdateSpell.ExecuteNonQuery();
                            }
                            else
                            {
                                commandInsertSpell.ExecuteNonQuery();
                            }
                        }
                    }

                    foreach (var itemSet in itemSetsFinal)
                    {
                        pItemSetId.Value = itemSet.Id != Guid.Empty ? itemSet.Id.ToString() : DBNull.Value;
                        pItemSetKiId.Value = itemSet.KiId != Guid.Empty ? itemSet.KiId.ToString() : DBNull.Value;
                        pItemSetName.Value = itemSet.SetName;
                        commandInsertItemSet.ExecuteNonQuery();

                        foreach (var bonus in itemSet.Bonuses)
                        {
                            InsertItem(bonus);
                        }
                    }
                    foreach (var item in itemsFinal)
                    {
                        InsertItem(item);
                    }

                    transaction.Commit();
                }

                db.Close();
            }
            return File.Exists(path);
        }
        public override bool Write(string path, IEnumerable<KiObject> values, bool append)
        {
            throw new NotImplementedException();
        }
        protected SqliteConnection _connection;

        internal static class SqliteQueries
        {
            public const string CreateItemTable = @"
            CREATE TABLE Items (
            ID                   TEXT NOT NULL UNIQUE,
            KI_ID                TEXT,
            Type                 INTEGER,
        	Name                 TEXT COLLATE BINARY,
        	LevelRequirement     INTEGER,
        	Flags                INTEGER,
        	PVPRankRequirement   INTEGER,
        	PetRankRequirement   INTEGER,
        	SchoolRequirement    INTEGER,
        	SchoolRestriction    INTEGER,
        	MaxHealth            INTEGER,
        	MaxMana              INTEGER,
        	MaxEnergy            INTEGER,
        	SpeedBonus           INTEGER,
        	PowerpipChance       INTEGER,
        	ShadowpipRating      INTEGER,
        	StunResistChance     INTEGER,
        	FishingLuck          INTEGER,
        	ArchmasteryRating    INTEGER,
        	IncomingHealing      INTEGER,
        	OutgoingHealing      INTEGER,
        	PipsGiven            INTEGER,
        	PowerpipsGiven       INTEGER,
        	AltSchoolMastery     INTEGER,
        	TearJewelSlots       INTEGER,
        	CircleJewelSlots     INTEGER,
        	SquareJewelSlots     INTEGER,
        	TriangleJewelSlots   INTEGER,
        	PowerPinSlots        INTEGER,
        	ShieldPinSlots       INTEGER,
        	SwordPinSlots        INTEGER,
        	SetBonus             TEXT COLLATE BINARY,
            KI_SetBonusID        TEXT,
        	SetBonusLevel        INTEGER,
        	PRIMARY KEY(ID),
        	FOREIGN KEY(SetBonus) REFERENCES ItemSets(ID)
        )";
            public const string CreateItemSetTable = @"
            CREATE TABLE ItemSets (
            ID    TEXT NOT NULL UNIQUE,
            KI_ID TEXT,
            Name  TEXT,
        	PRIMARY KEY(ID)
        )";
            public const string CreateComplexStatTable = @"
            CREATE TABLE ComplexStats (
            Type    INTEGER,
        	School  INTEGER,
        	Value   INTEGER,
        	Item    TEXT COLLATE BINARY,
        	PRIMARY KEY(Type, School, Value, Item),
        	FOREIGN KEY(Item) REFERENCES Items(ID)
        )";
            public const string CreateSpellTable = @"
            CREATE TABLE Spells (
            SpellName   TEXT COLLATE BINARY,
        	Quantity    INTEGER,
        	Item        TEXT COLLATE BINARY,
        	PRIMARY KEY(SpellName, Quantity, Item),
        	FOREIGN KEY(Item) REFERENCES Items(ID)
        )";

            public const string InsertItem = @"
            INSERT INTO Items (
                ID,
                KI_ID,
                Type,
                Name,
                LevelRequirement,
                Flags,
                PVPRankRequirement,
                PetRankRequirement,
                SchoolRequirement,
                SchoolRestriction,
                MaxHealth,
                MaxMana,
                MaxEnergy,
                SpeedBonus,
                PowerpipChance,
                ShadowpipRating,
                StunResistChance,
                FishingLuck,
                ArchmasteryRating,
                IncomingHealing,
                OutgoingHealing,
                PipsGiven,
                PowerpipsGiven,
                AltSchoolMastery,
                TearJewelSlots,
                CircleJewelSlots,
                SquareJewelSlots,
                TriangleJewelSlots,
                PowerPinSlots,
                ShieldPinSlots,
                SwordPinSlots,
                SetBonus,
                KI_SetBonusID,
                SetBonusLevel
            )
            VALUES (
                $ID,
                $KI_ID,
                $Type,
                $Name,
                $LevelRequirement,
                $Flags,
                $PVPRankRequirement,
                $PetRankRequirement,
                $SchoolRequirement,
                $SchoolRestriction,
                $MaxHealth,
                $MaxMana,
                $MaxEnergy,
                $SpeedBonus,
                $PowerpipChance,
                $ShadowpipRating,
                $StunResistChance,
                $FishingLuck,
                $ArchmasteryRating,
                $IncomingHealing,
                $OutgoingHealing,
                $PipsGiven,
                $PowerpipsGiven,
                $AltSchoolMastery,
                $TearJewelSlots,
                $CircleJewelSlots,
                $SquareJewelSlots,
                $TriangleJewelSlots,
                $PowerPinSlots,
                $ShieldPinSlots,
                $SwordPinSlots,
                $SetBonus,
                $KI_SetBonusID,
                $SetBonusLevel
            )";
            public const string InsertItemSet = @"
            INSERT INTO ItemSets
            (
                ID,
                KI_ID,
                Name
            )
            VALUES
            (
                $ID,
                $KI_ID,
                $Name
            )";
            public const string InsertComplexStat = @"
            INSERT INTO ComplexStats (
                Type,
                School,
                Value,
                Item
            )
            VALUES (
                $Type,
                $School,
                $Value,
                $Item
            )";
            public const string InsertSpell = @"
            INSERT INTO Spells
            (
                SpellName,
                Quantity,
                Item
            )
            VALUES
            (
                $SpellName,
                $Quantity,
                $Item
            )";

            public const string GetTableNames = @"
            SELECT name FROM sqlite_master WHERE type='table';
        ";

        }
    }
}
