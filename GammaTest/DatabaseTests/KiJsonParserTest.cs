using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GammaGear.Source;
using GammaGear.Source.Database;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace GammaTest.DatabaseTests
{
    [TestClass]
    public class KiJsonParserTest
    {
        private static string GetCurrentDirectory([CallerFilePath] string path = "")
        {
            return Path.GetDirectoryName(path);
        }

        [TestMethod]
        public void DeserializeRealFiles()
        {
            string localeLocation = Path.Combine(GetCurrentDirectory(), "Data", "Locale", "English");
            KiJsonParser<KiTextLocaleBank> kiJsonParser = new KiJsonParser<KiTextLocaleBank>(localeLocation);

            List<(string path, KiObject item)> testData = new List<(string, KiObject)>()
            {
                (
                    Path.Combine(GetCurrentDirectory(), "Data", "Json", "Amulet-AQ-Balance-Mastery.json"),
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
                    Path.Combine(GetCurrentDirectory(), "Data", "Json", "AP_Hamster_Death_TT_02.json"),
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
                    Path.Combine(GetCurrentDirectory(), "Data", "Json", "Athame-AQ-L90-001.json"),
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
                    Path.Combine(GetCurrentDirectory(), "Data", "Json", "Boots-AQ-Hades-L90-001.json"),
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
                    Path.Combine(GetCurrentDirectory(), "Data", "Json", "Deck-AQ-L90-001.json"),
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
                    Path.Combine(GetCurrentDirectory(), "Data", "Json", "Hat-AQ-Hades-L90-001.json"),
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
                    Path.Combine(GetCurrentDirectory(), "Data", "Json", "Jewel-S-75-Death-CB-8.json"),
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
            };


            foreach (var data in testData)
            {
                KiObject testItem = kiJsonParser.ReadToKiObject(data.path);
                Assert.IsTrue(testItem.Equals(data.item));
            }
        }
    }
}
