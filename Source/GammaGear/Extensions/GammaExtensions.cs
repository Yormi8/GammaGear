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
    }
}
