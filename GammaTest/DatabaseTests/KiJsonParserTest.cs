using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GammaGear.Source;
using GammaGear.Source.Database;
using System.Collections;


namespace GammaTest.DatabaseTests
{
    [TestClass]
    public class KiJsonParserTests
    {
        [TestMethod]
        public void DeserializeGameItems()
        {
            string localeLocation = Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Locale", "English");
            KiJsonParser<KiTextLocaleBank> kiJsonParser = new KiJsonParser<KiTextLocaleBank>(localeLocation);

            List<(string path, KiObject item)> testData = new List<(string, KiObject)>()
            {
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Amulet-AQ-Balance-Mastery.json"),
                    new Item()
                    {
                        Name = "Exalted Balance Amulet",
                        IncomingHealing = 2,
                        Blocks = new Dictionary<Item.School, int>
                        {
                            { Item.School.Any, 6 }
                        },
                        Damages = new Dictionary<Item.School, int>
                        {
                            { Item.School.Balance, 3 }
                        },
                        AltSchoolMastery = Item.School.Balance,
                        SchoolRestriction = Item.School.Balance,
                        SchoolRequirement = Item.School.Any,
                        TearJewelSlots = 1,
                        SquareJewelSlots = 1,
                        Type = Item.ItemType.Amulet,
                        Flags = Item.ItemFlags.FLAG_NoAuction,
                        LevelRequirement = 90
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "AP_Hamster_Death_TT_02.json"),
                    new Item()
                    {
                        Name = "Death Class Pet",
                        Type = Item.ItemType.Pet,
                        Flags = Item.ItemFlags.FLAG_NoAuction |
                                Item.ItemFlags.FLAG_NoSell |
                                Item.ItemFlags.FLAG_NoGift |
                                Item.ItemFlags.FLAG_CrownsOnly
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Athame-AQ-L90-001.json"),
                    new Item()
                    {
                        Name = "Blade of the Felled Titan",
                        Type = Item.ItemType.Athame,
                        LevelRequirement = 90,
                        MaxHealth = 320,
                        MaxMana = 210,
                        PowerpipChance = 17,
                        Blocks = new Dictionary<Item.School, int>
                        {
                            { Item.School.Any, 15 }
                        },
                        Damages = new Dictionary<Item.School, int>
                        {
                            { Item.School.Any, 15 }
                        },
                        OutgoingHealing = 17,
                        TearJewelSlots = 1,
                        CircleJewelSlots = 1,
                        TriangleJewelSlots = 1,
                        Flags = Item.ItemFlags.FLAG_NoAuction
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Boots-AQ-Hades-L90-001.json"),
                    new Item()
                    {
                        Name = "Hades' Firestriders",
                        Type = Item.ItemType.Boots,
                        LevelRequirement = 90,
                        SchoolRequirement = Item.School.Fire,
                        MaxHealth = 195,
                        PowerpipChance = 7,
                        Accuracies = new Dictionary<Item.School, int>()
                        {
                            { Item.School.Fire, 5 },
                        },
                        Blocks = new Dictionary<Item.School, int>()
                        {
                            { Item.School.Fire, 155 },
                        },
                        Damages = new Dictionary<Item.School, int>()
                        {
                            { Item.School.Fire, 12 },
                        },
                        Resists = new Dictionary<Item.School, int>()
                        {
                            { Item.School.Any, 6 },
                        },
                        Pierces = new Dictionary<Item.School, int>()
                        {
                            { Item.School.Any, 10 },
                        },
                        OutgoingHealing = 7,
                        Flags = Item.ItemFlags.FLAG_NoAuction
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Deck-AQ-L90-001.json"),
                    new Item()
                    {
                        Name = "Deck of Immortal Might",
                        Type = Item.ItemType.Deck,
                        LevelRequirement = 90,
                        MaxHealth = 50,
                        TearJewelSlots = 1,
                        ItemCards = new Dictionary<string, int>()
                        {
                            { "Fiery Giant - Amulet", 1 }
                        },
                        Flags = Item.ItemFlags.FLAG_NoAuction
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Hat-AQ-Hades-L90-001.json"),
                    new Item()
                    {
                        Name = "Hades' Crown of Blazes",
                        Type = Item.ItemType.Hat,
                        LevelRequirement = 90,
                        SchoolRequirement = Item.School.Fire,
                        MaxHealth = 235,
                        PowerpipChance = 7,
                        Accuracies = new Dictionary<Item.School, int>()
                        {
                            { Item.School.Fire, 4 }
                        },
                        Criticals = new Dictionary<Item.School, int>()
                        {
                            { Item.School.Fire, 175 }
                        },
                        Damages = new Dictionary<Item.School, int>()
                        {
                            { Item.School.Fire, 14 }
                        },
                        Pierces = new Dictionary<Item.School, int>()
                        {
                            { Item.School.Any, 5 }
                        },
                        OutgoingHealing = 3,
                        Flags = Item.ItemFlags.FLAG_NoAuction
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Jewel-S-75-Death-CB-8.json"),
                    new Item()
                    {
                        Name = "Opaque Blocking Onyx +8",
                        Type = Item.ItemType.SquareJewel,
                        LevelRequirement = 75,
                        Blocks = new Dictionary<Item.School, int>()
                        {
                            { Item.School.Death, 8 }
                        },
                        Flags = Item.ItemFlags.FLAG_NoAuction
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Mount-Narwhal-001.json"),
                    new Item()
                    {
                        Name = "Battle Narwhal (PERM)",
                        Type = Item.ItemType.Mount,
                        LevelRequirement = 1,
                        Damages = new Dictionary<Item.School, int>()
                        {
                            { Item.School.Any, 2 }
                        },
                        Flags = Item.ItemFlags.FLAG_NoAuction | Item.ItemFlags.FLAG_CrownsOnly
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Pin-DPS-160-LS-PD.json"),
                    new Item()
                    {
                        Name = "Storm Punishing Pin (160)",
                        Type = Item.ItemType.SwordPin,
                        LevelRequirement = 160,
                        SchoolRequirement = Item.School.Life,
                        Damages = new Dictionary<Item.School, int>()
                        {
                            { Item.School.Storm, 19 }
                        },
                        Pierces = new Dictionary<Item.School, int>()
                        {
                            { Item.School.Storm, 3 }
                        },
                        Flags = Item.ItemFlags.FLAG_NoAuction
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Ring-AQ-L90-001.json"),
                    new Item()
                    {
                        Name = "Alpha and Omega Ring",
                        Type = Item.ItemType.Ring,
                        LevelRequirement = 90,
                        SchoolRequirement = Item.School.Any,
                        MaxHealth = 370,
                        MaxMana = 135,
                        PowerpipChance = 14,
                        IncomingHealing = 17,
                        Blocks = new Dictionary<Item.School, int>()
                        {
                            { Item.School.Any, 30 }
                        },
                        Damages = new Dictionary<Item.School, int>()
                        {
                            { Item.School.Any, 10 }
                        },
                        TearJewelSlots = 1,
                        CircleJewelSlots = 1,
                        Flags = Item.ItemFlags.FLAG_NoAuction
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Robe-AQ-Hades-L90-001.json"),
                    new Item()
                    {
                        Name = "Hades' Inferno Plate",
                        Type = Item.ItemType.Robe,
                        LevelRequirement = 90,
                        SchoolRequirement = Item.School.Fire,
                        MaxHealth = 440,
                        PowerpipChance = 7,
                        Accuracies = new Dictionary<Item.School, int>()
                        {
                            { Item.School.Fire, 4 }
                        },
                        Criticals = new Dictionary<Item.School, int>()
                        {
                            { Item.School.Fire, 91 }
                        },
                        Damages = new Dictionary<Item.School, int>()
                        {
                            { Item.School.Fire, 18 }
                        },
                        Pierces = new Dictionary<Item.School, int>()
                        {
                            { Item.School.Any, 7 }
                        },
                        ItemCards = new Dictionary<string, int>()
                        {
                            { "Storm Lord Lava - Amulet", 1 }
                        },
                        OutgoingHealing = 9,
                        Flags = Item.ItemFlags.FLAG_NoAuction
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Wand-AQ-Ares-L30-001.json"),
                    new Item()
                    {
                        Name = "Sky Iron Hasta",
                        Type = Item.ItemType.Wand,
                        LevelRequirement = 30,
                        SchoolRequirement = Item.School.Any,
                        PowerpipsGiven = 1,
                        Damages = new Dictionary<Item.School, int>()
                        {
                            { Item.School.Any, 10 }
                        },
                        ItemCards = new Dictionary<string, int>()
                        {
                            { "Wand-Fire-T4-005", 6 }
                        },
                        Flags = Item.ItemFlags.FLAG_NoAuction
                    }
                ),
            };


            foreach (var data in testData)
            {
                KiObject testItem = kiJsonParser.ReadToKiObject(data.path);
                Assert.IsTrue(testItem.Equals(data.item), TestUtils.GetErrorString(testItem, data.item));
            }
        }

        [TestMethod]
        public void DeserializeGameItemSets()
        {
            string localeLocation = Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Locale", "English");
            KiJsonParser<KiTextLocaleBank> kiJsonParser = new KiJsonParser<KiTextLocaleBank>(localeLocation);

            List<(string path, KiObject item)> testData = new List<(string, KiObject)>()
            {
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "IS-L130-Death-001.json"),
                    new ItemSetBonus()
                    {
                        SetName = "Dragoon's Deadly Set",
                        Bonuses = new List<Item>()
                        {
                            new Item()
                            {
                                Name = "Dragoon's Deadly Set: Tier 2 Bonus",
                                Type = Item.ItemType.ItemSetBonusData,
                                Accuracies = new Dictionary<Item.School, int>()
                                {
                                    { Item.School.Death, 5 }
                                },
                                SetBonusLevel = 2
                            },
                            new Item()
                            {
                                Name = "Dragoon's Deadly Set: Tier 3 Bonus",
                                Type = Item.ItemType.ItemSetBonusData,
                                Damages = new Dictionary<Item.School, int>()
                                {
                                    { Item.School.Death, 4 }
                                },
                                PipConversions = new Dictionary<Item.School, int>()
                                {
                                    { Item.School.Death, 140 }
                                },
                                SetBonusLevel = 3
                            },
                            new Item()
                            {
                                Name = "Dragoon's Deadly Set: Tier 5 Bonus",
                                Type = Item.ItemType.ItemSetBonusData,
                                PowerpipChance = 12,
                                SetBonusLevel = 5
                            },
                            new Item()
                            {
                                Name = "Dragoon's Deadly Set: Tier 7 Bonus",
                                Type = Item.ItemType.ItemSetBonusData,
                                Damages = new Dictionary<Item.School, int>()
                                {
                                    { Item.School.Death, 6 }
                                },
                                Blocks = new Dictionary<Item.School, int>()
                                {
                                    { Item.School.Any, 108 }
                                },
                                StunResistChance = 25,
                                SetBonusLevel = 7
                            },
                        }
                    }
                )
            };

            foreach (var data in testData)
            {
                // This is super ugly, but I cannot think of a way to fix this without completely redesigning the item system
                // (which should probably be done anyway...)
                (data.item as ItemSetBonus)?.Bonuses.ForEach(i => i.SetBonus = data.item as ItemSetBonus);
                KiObject testItem = kiJsonParser.ReadToKiObject(data.path);
                Assert.IsTrue(testItem.Equals(data.item), TestUtils.GetErrorString(testItem, data.item));
            }
        }
    }
}
