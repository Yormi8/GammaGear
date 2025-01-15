using ABI.Windows.Storage.Streams;
using GammaItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GammaGear.Extensions
{
    public static class GammaExtensions
    {
        public static Uri ToIconUri(string iconName)
        {
            return iconName.ToLower() switch
            {
                "accuracy" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Accuracy.png", UriKind.Absolute),
                "archmastery" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Archmastery.png", UriKind.Absolute),
                "armorpiercing" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Armor_Piercing.png", UriKind.Absolute),
                "critical" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Critical.png", UriKind.Absolute),
                "criticalblock" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Critical_Block.png", UriKind.Absolute),
                "damage" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Damage.png", UriKind.Absolute),
                "energy" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Energy.png", UriKind.Absolute),
                "fishingluck" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Fishing_Luck.png", UriKind.Absolute),
                "flatdamage" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Flat_Damage.png", UriKind.Absolute),
                "flatresistance" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Flat_Resistance.png", UriKind.Absolute),
                "health" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Health.png", UriKind.Absolute),
                "incoming" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Incoming.png", UriKind.Absolute),
                "mana" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Mana.png", UriKind.Absolute),
                "outgoing" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Outgoing.png", UriKind.Absolute),
                "pip" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Pip.png", UriKind.Absolute),
                "pipconversion" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Pip_Conversion.png", UriKind.Absolute),
                "powerpip" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Power_Pip.png", UriKind.Absolute),
                "resistance" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Resistance.png", UriKind.Absolute),
                "shadowpip" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Shadow_Pip.png", UriKind.Absolute),
                "speedbonus" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_SpeedBonus.png", UriKind.Absolute),
                "stunresistance" => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Stats_Stun_Resistance.png", UriKind.Absolute),
                _ => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Button_Trash.png", UriKind.Absolute)
            };
        }

        public static Uri ToIconUri(this School school)
        {
            return school switch
            {
                School.Fire => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Fire.png", UriKind.Absolute),
                School.Ice => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Ice.png", UriKind.Absolute),
                School.Storm => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Storm.png", UriKind.Absolute),
                School.Myth => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Myth.png", UriKind.Absolute),
                School.Life => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Life.png", UriKind.Absolute),
                School.Death => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Death.png", UriKind.Absolute),
                School.Balance => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Balance.png", UriKind.Absolute),
                School.Shadow => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Shadow.png", UriKind.Absolute),
                _ => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Global.png", UriKind.Absolute),
            };
        }

        public static Uri ToSlashedIconUri(this School school)
        {
            return school switch
            {
                School.Fire => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Not_Fire.png", UriKind.Absolute),
                School.Ice => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Not_Ice.png", UriKind.Absolute),
                School.Storm => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Not_Storm.png", UriKind.Absolute),
                School.Myth => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Not_Myth.png", UriKind.Absolute),
                School.Life => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Not_Life.png", UriKind.Absolute),
                School.Death => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Not_Death.png", UriKind.Absolute),
                School.Balance => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Not_Balance.png", UriKind.Absolute),
                School.Shadow => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Not_Shadow.png", UriKind.Absolute),
                _ => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Button_Trash.png", UriKind.Absolute),
            };
        }

        public static Uri ToIconUri(this ItemType type)
        {
            return type switch
            {
                ItemType.Hat => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_Hat.png"),
                ItemType.Robe => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_Robe.png"),
                ItemType.Shoes => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_Boots.png"),
                ItemType.Weapon => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_Wand.png"),
                ItemType.Athame => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_Athame.png"),
                ItemType.Amulet => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_Amulet.png"),
                ItemType.Ring => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_Ring.png"),
                ItemType.Deck => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_Deck.png"),
                ItemType.Pet => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_Pet.png"),
                ItemType.Mount => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_Mount.png"),
                ItemType.TearJewel => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_TearJewel_Filled.png"),
                ItemType.CircleJewel => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_CircleJewel_Filled.png"),
                ItemType.SquareJewel => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_SquareJewel_Filled.png"),
                ItemType.TriangleJewel => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_TriangleJewel_Filled.png"),
                ItemType.PinSquarePip => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_PinSquarePower_Filled.png"),
                ItemType.PinSquareShield => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_PinSquareShield_Filled.png"),
                ItemType.PinSquareSword => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Equipment_PinSquareSword_Filled.png"),
                ItemType.None => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_School_Global.png"),
                _ => new Uri(@"pack://application:,,,/GammaGear;component/Assets/Images/(Icon)_Button_Trash.png", UriKind.Absolute),
            };
        }

        public static ItemLoadout GenerateRandomLoadout()
        {
            Random random = new Random();
            ItemLoadout loadout = new ItemLoadout();

            List<string> names = new List<string>()
            {
                "Merle Ambrose",
                "Gamma",
                "Dalia Falmea",
                "Belladonna Crisp",
                "Sylvia Drake",
                "Dryad",
                "Halston Balestrom",
                "Vlad Raveneye",
                "Selena Gomez",
                "Greyhorn Mercenary",
                "Morganthe",
                "Baba Yaga"
            };

            List<string> title1 = new List<string>()
            {
                "Cool ",
                "Dope ",
                "Super ",
                "Awesome ",
                "Epic ",
                "Legendary "
            };

            List<string> title2 = new List<string>()
            {
                "Loadout ",
                "Gear Set ",
                "Clothings ",
                "Backpack ",
                "Dream Suit ",
                "Fit "
            };

            List<string> title3 = new List<string>()
            {
                "of Wizard City",
                "of Krokotopia",
                "of Marleybone",
                "of Mooshu",
                "of Dragonspyre",
                "of Grizzleheim"
            };

            loadout.Creator = names[random.Next(names.Count)];
            loadout.Name = title1[random.Next(title1.Count)] + title2[random.Next(title2.Count)] + title3[random.Next(title3.Count)];
            loadout.WizardName = names[random.Next(names.Count)];
            loadout.WizardLevel = random.Next(1, 161);
            loadout.WizardSchool = (School)random.Next(1, 8);
            loadout.TimeCreated = DateTime.Now + new TimeSpan(random.Next(730) - 365, random.Next(48) - 24, random.Next(120) - 60, random.Next(120) - 60);
            loadout.TimeUpdated = loadout.TimeCreated + new TimeSpan(random.Next(10), 0, 0, 0);
            return loadout;
        }

        public static Item GenerateRandomItem()
        {
            Random rand = new Random((int)System.DateTime.Now.ToUniversalTime().Ticks);
            Item item = new Item()
            {
                MaxHealth = rand.Next(0, 2) == 1 ? rand.Next(1, 501) : 0,
                MaxMana = rand.Next(0, 2) == 1 ? rand.Next(1, 501) : 0,
                MaxEnergy = rand.Next(0, 2) == 1 ? rand.Next(1, 501) : 0,
                ArchmasteryRating = rand.Next(0, 2) == 1 ? rand.Next(1, 501) : 0,
                FishingLuck = rand.Next(0, 2) == 1 ? rand.Next(1, 101) : 0,
                IncomingHealing = rand.Next(0, 2) == 1 ? rand.Next(1, 101) : 0,
                OutgoingHealing = rand.Next(0, 2) == 1 ? rand.Next(1, 101) : 0,
                LevelRequirement = rand.Next(0, 2) == 1 ? rand.Next(1, 17) * 10 : 1,
                SchoolRequirement = (School)rand.Next((int)School.Any, (int)School.Shadow),
                PowerpipChance = rand.Next(0, 2) == 1 ? rand.Next(1, 22) : 1,
                ShadowpipRating = rand.Next(0, 2) == 1 ? rand.Next(1, 21) : 1,
                StunResistChance = rand.Next(0, 101),
                Type = (ItemType)rand.Next((int)ItemType.Hat, (int)ItemType.ItemSetBonusData),
                TearJewelSlots = rand.Next(0, 3),
                CircleJewelSlots = rand.Next(0, 3),
                SquareJewelSlots = rand.Next(0, 3),
                TriangleJewelSlots = rand.Next(0, 3),
                PowerPinSlots = rand.Next(0, 3),
                ShieldPinSlots = rand.Next(0, 3),
                SwordPinSlots = rand.Next(0, 3),
            };

            item.SchoolRestriction = item.SchoolRequirement != School.Any ? School.None : (School)rand.Next((int)School.Any, (int)School.Shadow);
            item.Accuracies.Add(rand.Next(0, 2) == 0 ? School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.Blocks.Add(rand.Next(0, 2) == 0 ? School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.Damages.Add(rand.Next(0, 2) == 0 ? School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.Criticals.Add(rand.Next(0, 2) == 0 ? School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.FlatDamages.Add(rand.Next(0, 2) == 0 ? School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.FlatResists.Add(rand.Next(0, 2) == 0 ? School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.Pierces.Add(rand.Next(0, 2) == 0 ? School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.PipConversions.Add(rand.Next(0, 2) == 0 ? School.Any : item.SchoolRequirement, rand.Next(1, 20));
            item.Resists.Add(rand.Next(0, 2) == 0 ? School.Any : item.SchoolRequirement, rand.Next(1, 20));


            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string name = "";
            name += consonants[rand.Next(consonants.Length)].ToUpper();
            name += vowels[rand.Next(vowels.Length)];
            int b = rand.Next(8, 31); //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b > 0)
            {
                name += consonants[rand.Next(consonants.Length)];
                b--;
                name += vowels[rand.Next(vowels.Length)];
                b--;
            }

            item.Name = name;

            return item;
        }
    }
}
