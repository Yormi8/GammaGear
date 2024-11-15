using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GammaItems.Source.Database;
using GammaItems;
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

            List<(string path, ItemBase item)> testData = new List<(string, ItemBase)>()
            {
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Amulet-AQ-Balance-Mastery.json"),
                    new Item()
                    {
                        Name = "Exalted Balance Amulet",
                        IncomingHealing = 2,
                        Blocks = new Dictionary<School, int>
                        {
                            { School.Any, 6 }
                        },
                        Damages = new Dictionary<School, int>
                        {
                            { School.Balance, 3 }
                        },
                        AltSchoolMasteries = new(){ School.Balance },
                        SchoolRestriction = School.Balance,
                        SchoolRequirement = School.Any,
                        TearJewelSlots = 1,
                        SquareJewelSlots = 1,
                        Type = ItemType.Amulet,
                        Flags = ItemFlags.FLAG_NoAuction,
                        LevelRequirement = 90
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "AP_Hamster_Death_TT_02.json"),
                    new Item()
                    {
                        Name = "Death Class Pet",
                        Type = ItemType.Pet,
                        Flags = ItemFlags.FLAG_NoAuction |
                                ItemFlags.FLAG_NoSell |
                                ItemFlags.FLAG_NoGift |
                                ItemFlags.FLAG_CrownsOnly
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Athame-AQ-L90-001.json"),
                    new Item()
                    {
                        Name = "Blade of the Felled Titan",
                        Type = ItemType.Athame,
                        LevelRequirement = 90,
                        MaxHealth = 320,
                        MaxMana = 210,
                        PowerpipChance = 17,
                        Blocks = new Dictionary<School, int>
                        {
                            { School.Any, 15 }
                        },
                        Damages = new Dictionary<School, int>
                        {
                            { School.Any, 15 }
                        },
                        OutgoingHealing = 17,
                        TearJewelSlots = 1,
                        CircleJewelSlots = 1,
                        TriangleJewelSlots = 1,
                        Flags = ItemFlags.FLAG_NoAuction
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Boots-AQ-Hades-L90-001.json"),
                    new Item()
                    {
                        Name = "Hades' Firestriders",
                        Type = ItemType.Boots,
                        LevelRequirement = 90,
                        SchoolRequirement = School.Fire,
                        MaxHealth = 195,
                        PowerpipChance = 7,
                        Accuracies = new Dictionary<School, int>()
                        {
                            { School.Fire, 5 },
                        },
                        Blocks = new Dictionary<School, int>()
                        {
                            { School.Fire, 155 },
                        },
                        Damages = new Dictionary<School, int>()
                        {
                            { School.Fire, 12 },
                        },
                        Resists = new Dictionary<School, int>()
                        {
                            { School.Any, 6 },
                        },
                        Pierces = new Dictionary<School, int>()
                        {
                            { School.Any, 10 },
                        },
                        OutgoingHealing = 7,
                        Flags = ItemFlags.FLAG_NoAuction
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Deck-AQ-L90-001.json"),
                    new Item()
                    {
                        Name = "Deck of Immortal Might",
                        Type = ItemType.Deck,
                        LevelRequirement = 90,
                        MaxHealth = 50,
                        TearJewelSlots = 1,
                        ItemCards = new Dictionary<string, int>()
                        {
                            { "Fiery Giant - Amulet", 1 }
                        },
                        Flags = ItemFlags.FLAG_NoAuction
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Hat-AQ-Hades-L90-001.json"),
                    new Item()
                    {
                        Name = "Hades' Crown of Blazes",
                        Type = ItemType.Hat,
                        LevelRequirement = 90,
                        SchoolRequirement = School.Fire,
                        MaxHealth = 235,
                        PowerpipChance = 7,
                        Accuracies = new Dictionary<School, int>()
                        {
                            { School.Fire, 4 }
                        },
                        Criticals = new Dictionary<School, int>()
                        {
                            { School.Fire, 175 }
                        },
                        Damages = new Dictionary<School, int>()
                        {
                            { School.Fire, 14 }
                        },
                        Pierces = new Dictionary<School, int>()
                        {
                            { School.Any, 5 }
                        },
                        OutgoingHealing = 3,
                        Flags = ItemFlags.FLAG_NoAuction
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Jewel-S-75-Death-CB-8.json"),
                    new Item()
                    {
                        Name = "Opaque Blocking Onyx +8",
                        Type = ItemType.SquareJewel,
                        LevelRequirement = 75,
                        Blocks = new Dictionary<School, int>()
                        {
                            { School.Death, 8 }
                        },
                        Flags = ItemFlags.FLAG_NoAuction
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Mount-Narwhal-001.json"),
                    new Item()
                    {
                        Name = "Battle Narwhal (PERM)",
                        Type = ItemType.Mount,
                        LevelRequirement = 1,
                        Damages = new Dictionary<School, int>()
                        {
                            { School.Any, 2 }
                        },
                        Flags = ItemFlags.FLAG_NoAuction | ItemFlags.FLAG_CrownsOnly
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Pin-DPS-160-LS-PD.json"),
                    new Item()
                    {
                        Name = "Storm Punishing Pin (160)",
                        Type = ItemType.SwordPin,
                        LevelRequirement = 160,
                        SchoolRequirement = School.Life,
                        Damages = new Dictionary<School, int>()
                        {
                            { School.Storm, 19 }
                        },
                        Pierces = new Dictionary<School, int>()
                        {
                            { School.Storm, 3 }
                        },
                        Flags = ItemFlags.FLAG_NoAuction
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Ring-AQ-L90-001.json"),
                    new Item()
                    {
                        Name = "Alpha and Omega Ring",
                        Type = ItemType.Ring,
                        LevelRequirement = 90,
                        SchoolRequirement = School.Any,
                        MaxHealth = 370,
                        MaxMana = 135,
                        PowerpipChance = 14,
                        IncomingHealing = 17,
                        Blocks = new Dictionary<School, int>()
                        {
                            { School.Any, 30 }
                        },
                        Damages = new Dictionary<School, int>()
                        {
                            { School.Any, 10 }
                        },
                        TearJewelSlots = 1,
                        CircleJewelSlots = 1,
                        Flags = ItemFlags.FLAG_NoAuction
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Robe-AQ-Hades-L90-001.json"),
                    new Item()
                    {
                        Name = "Hades' Inferno Plate",
                        Type = ItemType.Robe,
                        LevelRequirement = 90,
                        SchoolRequirement = School.Fire,
                        MaxHealth = 440,
                        PowerpipChance = 7,
                        Accuracies = new Dictionary<School, int>()
                        {
                            { School.Fire, 4 }
                        },
                        Criticals = new Dictionary<School, int>()
                        {
                            { School.Fire, 91 }
                        },
                        Damages = new Dictionary<School, int>()
                        {
                            { School.Fire, 18 }
                        },
                        Pierces = new Dictionary<School, int>()
                        {
                            { School.Any, 7 }
                        },
                        ItemCards = new Dictionary<string, int>()
                        {
                            { "Storm Lord Lava - Amulet", 1 }
                        },
                        OutgoingHealing = 9,
                        Flags = ItemFlags.FLAG_NoAuction
                    }
                ),
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "Wand-AQ-Ares-L30-001.json"),
                    new Item()
                    {
                        Name = "Sky Iron Hasta",
                        Type = ItemType.Wand,
                        LevelRequirement = 30,
                        SchoolRequirement = School.Any,
                        PowerpipsGiven = 1,
                        Damages = new Dictionary<School, int>()
                        {
                            { School.Any, 10 }
                        },
                        ItemCards = new Dictionary<string, int>()
                        {
                            { "Wand-Fire-T4-005", 6 }
                        },
                        Flags = ItemFlags.FLAG_NoAuction
                    }
                ),
            };


            foreach (var data in testData)
            {
                ItemBase testItem = kiJsonParser.ReadToItemBase(data.path);
                Assert.IsTrue(testItem.Equals(data.item), TestUtils.GetErrorString(testItem, data.item));
            }
        }

        [TestMethod]
        public void DeserializeGameItemSets()
        {
            string localeLocation = Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Locale", "English");
            KiJsonParser<KiTextLocaleBank> kiJsonParser = new KiJsonParser<KiTextLocaleBank>(localeLocation);

            List<(string path, ItemBase item)> testData = new List<(string, ItemBase)>()
            {
                (
                    Path.Combine(TestUtils.GetCurrentDirectory(), "Data", "Json", "IS-L130-Death-001.json"),
                    new ItemSet()
                    {
                        SetName = "Dragoon's Deadly Set",
                        Bonuses = new List<ItemSetBonus>()
                        {
                            new ItemSetBonus()
                            {
                                Name = "Dragoon's Deadly Set: Tier 2 Bonus",
                                Type = ItemType.ItemSetBonusData,
                                Accuracies = new Dictionary<School, int>()
                                {
                                    { School.Death, 5 }
                                },
                                SetBonusLevel = 2
                            },
                            new ItemSetBonus()
                            {
                                Name = "Dragoon's Deadly Set: Tier 3 Bonus",
                                Type = ItemType.ItemSetBonusData,
                                Damages = new Dictionary<School, int>()
                                {
                                    { School.Death, 4 }
                                },
                                PipConversions = new Dictionary<School, int>()
                                {
                                    { School.Death, 140 }
                                },
                                SetBonusLevel = 3
                            },
                            new ItemSetBonus()
                            {
                                Name = "Dragoon's Deadly Set: Tier 5 Bonus",
                                Type = ItemType.ItemSetBonusData,
                                PowerpipChance = 12,
                                SetBonusLevel = 5
                            },
                            new ItemSetBonus()
                            {
                                Name = "Dragoon's Deadly Set: Tier 7 Bonus",
                                Type = ItemType.ItemSetBonusData,
                                Damages = new Dictionary<School, int>()
                                {
                                    { School.Death, 6 }
                                },
                                Blocks = new Dictionary<School, int>()
                                {
                                    { School.Any, 108 }
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
                (data.item as ItemSet)?.Bonuses.ForEach(i => i.SetBonus = data.item as ItemSet);
                ItemBase testItem = kiJsonParser.ReadToItemBase(data.path);
                Assert.IsTrue(testItem.Equals(data.item), TestUtils.GetErrorString(testItem, data.item));
            }
        }
    }
}
