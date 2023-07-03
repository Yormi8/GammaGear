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

            KiObject testItem = kiJsonParser.ReadToKiObject(
                Path.Combine(GetCurrentDirectory(), "Data", "Json", "Amulet-AQ-Balance-Mastery.json"));

            KiObject testCompare = new Item()
            {
                Name = "Exalted Balance Amulet",
                IncomingHealing = 2,
                Blocks = new Dictionary<Item.School, int>
                {
                    { Item.School.Any, 2 }
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
            };

            Assert.IsTrue(testItem.Equals(testCompare));
        }
    }
}
