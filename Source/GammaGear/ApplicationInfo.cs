using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GammaGear
{
    public static class ApplicationInfo
    {
        public static IReadOnlyDictionary<int, string> AppCodenameDict => new Dictionary<int, string>()
        {
            // FISMLDB
            { 0, "Fire Cat" },
            { 1, "Frost Beetle" },
            { 2, "Thunder Snake" },
            { 3, "Blood Bat" },
            { 4, "Imp" },
            { 5, "Dark Sprite" },
            { 6, "Scarab" },
            { 7, "Fire Elf" },
            { 8, "Snow Serpent" },
            { 9, "Lightning Bats" },
            { 10, "Troll" },
            { 11, "Leprechaun" },
            { 12, "Ghoul" },
            { 13, "Scorpion" },
        };
        public static Version AppVersion => Assembly.GetExecutingAssembly().GetName().Version;
        public static string AppCodename => AppCodenameDict.TryGetValue(AppVersion.Major, out string name) ? name : "ERROR CODENAME";
        public static string AppTitle => "Gamma Gear";
        public static string AppDisplayTitle => $"{AppTitle} ({AppCodename})";
        public static string AppDisplayVersion => $"{AppDisplayTitle} v{AppVersion}";
    }
}
